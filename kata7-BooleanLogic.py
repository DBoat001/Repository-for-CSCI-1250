# Boolean logic from scratch
# https://www.codewars.com/kata/584d2c19766c2b2f6a00004f
''' func_xor(a, b) checks if exactly one of the two values is truthy.
 It returns True if one is true and the other is false.
 func_or(a, b) checks if at least one of the two values is truthy. 
 It returns True if either or both are true.
 bool(a) and bool(b) convert any input (like numbers, strings, lists) 
 into True or False.
'''



def func_xor(a, b):
    # True if exactly one is truthy
    return bool(a) != bool(b)

def func_or(a, b):
    # True if at least one is truthy
    return bool(a) + bool(b) > 0