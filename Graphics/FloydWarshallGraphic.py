import numpy as np
import matplotlib.pyplot as plt
from scipy.interpolate import make_interp_spline

x = np.array([0, 1, 2, 4, 5])
y = np.array([1, 4, 24,69.3, 129 + 3.2])

def yt(xt):
    return xt ** 3

spline = make_interp_spline(x, y)

# Create an array of x values representing the curve
xnew = np.linspace(x.min(), x.max(), 500)

# Create the plot
plt.plot(xnew, spline(xnew), 'r-', x, y, 'bo')

xtlin = np.linspace(x.min(), x.max() , 500)
plt.plot(xtlin, yt(xtlin), 'g-')
# Show the plot
plt.show()