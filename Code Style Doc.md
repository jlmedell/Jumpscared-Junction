# **Indentation & Spacing**

We will use tabs to indent our code.

Try to keep indents aligned, one tab increase at a time.

# **Naming Conventions**

For our variables, functions, and classes, we will use camelCase.

```
Vector3 playerMovement
Function playerMovementSpeed()
```
Try to be as descriptive as possible with names. Make the names as long or as short as we need to get information across.
```
Function playerMovementSpeed() instead of Function movementSpeed()
```

# Brace Styling

We will use Allman style braces.
```
if(i > 0)
{
		return("yay");
}
```

# Formula Formatting
Use parentheses where possible for clarity. The last operation in an expression does NOT need to be inside parenthesis.
```
x = i * (y + z)
```
Try making functions for repeatedly used formulas (optimize code)
```
Function damageToPlayerHealth()
Function calculateCurrentPlayerSpeed()
```

# Code Readability
Include comments where appropriate, try to keep them concise but clear.
Keep comments primarily above major blocks of code, not inline.
Major blocks of code should have a descriptor explaining what it is used for.
```
//Variables for player movement
//Helper function for player health
//Player movement functionality
```
Use /**/ for multi-line comments.
```
/*
This function is used to calculater the player's movement speed.
It takes a Vector3 representing the player's body
It returns a float representing the calculated movement speed.
*/
```
All code is to be writen in VS Code

# Language Standard Compliance
Using documentation for version 6.3 of Unity for C# and other Unity related matters.
https://docs.unity3d.com/6000.3/Documentation/Manual/index.html



