namespace Isu.Extra.Models;

public class Time
{
    private int _hour;
    private int _minute;

    public Time(int hour, int minute)
    {
        if (hour < 0 || hour > 23)
            throw new Exception(); // TODO
        if (minute < 0 || minute > 59)
            throw new Exception(); // TODO
        _hour = hour;
        _minute = minute;
    }

    public int Hour => _hour;
    public int Minute => _minute;

    public static bool operator <(Time time1, Time time2)
    {
        return time1._hour < time2._hour || (time1._hour == time2._hour && time1._minute < time2._minute);
    }

    public static bool operator >(Time time1, Time time2)
    {
        return time1._hour > time2._hour || (time1._hour == time2._hour && time1._minute > time2._minute);
    }
}