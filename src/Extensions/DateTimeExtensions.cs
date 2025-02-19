using System.Globalization;
using System.Runtime.CompilerServices;

namespace SixTatami.Extensions;

public static class DateTimeExtensions {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetWeekOfYear(this DateTime date) {
		var cal = CultureInfo.InvariantCulture.Calendar;
		return cal.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetISOWeekNumber(this DateTime date) => ISOWeek.GetWeekOfYear(date);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime GetStartDateOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Sunday) {
		int diff = (7 + date.DayOfWeek - startOfWeek) % 7;
		return date.AddDays(-diff).Date;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime GetEndDateOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Sunday) {
		int diff = (6 + startOfWeek - date.DayOfWeek) % 7;
		return date.AddDays(diff).Date;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime GetStartDateOfWeek(int year, int week, DayOfWeek startOfWeek = DayOfWeek.Sunday) {
		var jan1 = new DateTime(year, 1, 1);
		var startDateOfFirstWeek = jan1.GetStartDateOfWeek(startOfWeek);
		return startDateOfFirstWeek.AddDays((week - 1) * 7);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime GetEndDateOfWeek(int year, int week, DayOfWeek startOfWeek = DayOfWeek.Sunday) {
		var jan1 = new DateTime(year, 1, 1);
		var startDateOfFirstWeek = jan1.GetStartDateOfWeek(startOfWeek);
		return startDateOfFirstWeek.AddDays((week - 1) * 7 + 6);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime ToDateTime(int year, int weekNumber, DayOfWeek dayOfWeek) => ISOWeek.ToDateTime(year, weekNumber, dayOfWeek);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime GetStartDateOfMonth(this DateTime date) {
		var year = date.Year;
		var month = date.Month;
		var day = 1;
		return new DateTime(year, month, day);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime GetEndDateOfMonth(this DateTime date) {
		var year = date.Year;
		var month = date.Month;
		var day = DateTime.DaysInMonth(year, month);
		return new DateTime(year, month, day);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetQuarter(this DateTime date) => (date.Month + 2) / 3;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime GetStartDateOfQuarter(this DateTime date) {
		var quarter = date.GetQuarter();
		var startMonth = quarter * 3 - 2;
		var year = date.Year;
		return new DateTime(year, startMonth, 1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime GetEndDateOfQuarter(this DateTime date) {
		var quarter = date.GetQuarter();
		var endMonth = quarter * 3;
		var year = date.Year;
		var day = DateTime.DaysInMonth(year, endMonth);
		return new DateTime(year, endMonth, day);
	}
}
