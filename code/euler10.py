# This problem uses Python to solve.  The problem statement is below.





# The sum of the primes below 10 is 2 + 3 + 5 + 7 = 17.
#
# Find the sum of all the primes below two million.
import math
import time

# The following variable is used to test the efficiency of the algorithm.

start = time.time()

# The following function tests the 'prime-ness' of any given positive integer.

def isPrime(x):
    prime = True
    for i in range(2, int(math.sqrt(x)) + 1):
        if x % i == 0:
            prime = False
            break
    return prime



# The following function collects the sum of all prime numbers below x.
def primeSum(x):
    if x < 2 or type(x) != int:
        return "Type Error. Check your input."
    elif x == 2:
        return 2
    elif x == 3:
        return 5

    sum = 2
    for i in range(3, x):
        if isPrime(i):
            sum += i
    return sum

# The following line will show how long it takes to complete the algorithm.
print("Completed in %s seconds" % (time.time() - start))
