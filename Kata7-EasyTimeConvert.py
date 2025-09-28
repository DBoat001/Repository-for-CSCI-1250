# Easy Time Convert
# https://www.codewars.com/kata/5a084a098ba9146690000969
''' The function first checks if the input minutes is 0 or negative. 
If so, it returns "00:00".
Otherwise, it divides the minutes by 60 to find the number of full hours.
It then uses the remainder (%) to find the leftover minutes.
Finally, it formats the hours and minutes as a string in "hh:mm" with two digits each. 
'''


def time_convert(minutes):
    if minutes <= 0:
        return "00:00"

    hours = minutes // 60        # whole hours
    mins  = minutes % 60         # remainder minutes

    return f"{hours:02d}:{mins:02d}"