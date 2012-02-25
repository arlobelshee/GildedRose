using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using GildedRose.Console;
using FluentAssertions;

namespace GildedRose.Tests
{
	[TestFixture, UseReporter(typeof(QuietReporter))]
	public class ItemAgingRules
	{
		[Test]
		public void LegacyShouldContinueToBehaveAsItAlwaysDid()
		{
			var testSubject = new Console.GildedRose();
			var trace = new StringBuilder();
			for(int i = 0; i < 50; ++i)
			{
				trace.AppendLine(DisplayInventory(testSubject.InventoryTestAccess));
				testSubject.UpdateQuality();
			}
			trace.AppendLine(DisplayInventory(testSubject.InventoryTestAccess));
			Approvals.Verify(trace.ToString());
		}

		[Test]
		public void ItemQualityShouldAlwaysRemainWithinAllowedMax()
		{
			var testSubject = new Console.GildedRose.Item {Quality = 3};
			testSubject.AdjustQuality(90);
			testSubject.Quality.Should().Be(50);
		}

		[Test]
		public void ItemQualityShouldAlwaysRemainWithinAllowedMin()
		{
			var testSubject = new Console.GildedRose.Item {Quality = 3};
			testSubject.AdjustQuality(-90);
			testSubject.Quality.Should().Be(0);
		}

		private string DisplayInventory(IEnumerable<Console.GildedRose.Item> inventory)
		{
			return string.Join("\r\n",inventory.Select(DisplayItem)) + "\r\n";
		}

		private string DisplayItem(Console.GildedRose.Item it)
		{
			return string.Format("{0}, q={1}, sell in {2} days", it.Name, it.Quality, it.SellIn);
		}
	}
}
