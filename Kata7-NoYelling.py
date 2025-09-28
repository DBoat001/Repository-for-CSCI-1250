# No Yelling!
# https://www.codewars.com/kata/587a75dbcaf9670c32000292
'''The function takes a string as input. 
It removes extra spaces between words and trims spaces at the start and end.
converts all letters to lowercase and then capitalizes the first letter of the string.
Finally, it returns the cleaned, properly spaced, and capitalized string.
'''



def filter_words(text):
    # Remove extra spaces and split into words
    words = text.split()
    
    # Join the words with a single space
    cleaned = " ".join(words)
    
    # Convert all letters to lowercase and capitalize the first letter
    return cleaned.lower().capitalize()