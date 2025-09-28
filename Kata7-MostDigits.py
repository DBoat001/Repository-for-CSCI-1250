# Most Digits
# https://www.codewars.com/kata/58daa7617332e59593000006
'''The function takes a list of numbers as input.
It converts each number to a string to count how many digits it has.
Finally, it returns the number that has the most digits.
'''




def find_longest(numbers):
    # start with the first number as the longest
    longest = numbers[0]
    
    for num in numbers:
        # compare lengths by converting to string
        if len(str(num)) > len(str(longest)):
            longest = num
    
    return longest
