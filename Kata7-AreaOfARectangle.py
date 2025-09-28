# Find the area of the rectangle!
# https://www.codewars.com/kata/580a0347430590220e000091
'''First, it checks if the diagonal is less than or equal to the side. If so, 
it returns "Not a rectangle" since that's not possible.
If valid, it uses Pythagoras’ theorem to calculate the missing side of the rectangle.
Then it multiplies the two sides to get the area.
Finally, it rounds the result to two decimal places and returns it.
'''


def area(diagonal, side):
    # if the diagonal is too short, it's not a valid rectangle
    if diagonal <= side:
        return "Not a rectangle"
    
    # use Pythagoras theorem to find the other side
    otherSide = (diagonal**2 - side**2) ** 0.5
    
    # calculate area
    result = side * otherSide
    
    # round to 2 decimal places
    return round(result, 2)
