# BGRA32Changer
#### This program supports adjusting image's BGRA value and merge linearly plural images to one image file.

<br/>

## Guides
<br/>

### 1. Overview
    When you execute the program, you can see the program like this.

<img src="/Guide images/1.png"/>

    In the white space at the top of the screen, there's an image that we've brought up.

    At the bottom left of the screen, you can see a slider that allows you to change the values of Red, Green, Blue, and Alpha.
    You can change the overall color of the image by manipulating the slider.

    On the right side of the screen,
    Image import button and number of imported images
    Boxes for checking and changing background colors
    Sliders that allow you to change the background color Alpha value
    There is a box where you can set the number of images that are printed per line with the Save Image button.
    
### 2. Load Images 
<img src="/Guide images/2.png"/>

    Let's load the image through the Load Images button at the bottom right of the screen.
<img src="/Guide images/3.png"/>

    If the image came out well, it's a success!
    Now let's click on the image to change the color.
    
### 3. Let's change the color.
    Now let's change the color of the image.
    First, let's change the color by operating the red slider on the bottom left.
<img src="/Guide images/4.png"/>

<img src="/Guide images/5.png"/>
    
    Moving the slider to the right adds as much value as you moved it to the image.

    In particular, if only black images exist on a white background, the background color remains white (#FFFFFFFF)
    You can see that only the color of the line changes because the value is added only to the color of the line (#000000).

    If you move the slider to the left in the above situation, the color of the line (#000000) is no longer subtractable
    You can see the effect of changing the background color because it reduces the value only in the background color (#FFFFFFFF).

    If you manipulate the R, G, and B values to the right by the same value, the overall brightness of the image increases.
    (Conversely, if you operate to the left by the same value, the brightness decreases.)
    
### 4. Change the transparency!
    You can also change the transparency of the image by operating the Alpha slider on the lower left.
<img src="/Guide images/6.png"/>
    
    If a transparent image is loaded, the slider can be operated to the right to create an opaque image.
### 5. Let's change the background color!
    If the image you are currently modifying is transparent, the background color may be important.

    This background color change feature makes it easy to create images with specific colors added.

    Let's click on the Background Colour box at the bottom right. (The default color is white.)
<img src="/Guide images/7.png"/>

    Set the desired color and press the OK button to change the background color behind the image.
<img src="/Guide images/8.png"/>

    The color is applied to the image together when you save the image.
### 6. Let's make a transparent image by changing the transparency of the background color.
    If you want to create a transparent image with a background color, let's manipulate the Alpha slider at the bottom right.
<img src="/Guide images/9.png"/>

    If you operate the slider to the left, the background color is also transparent, and the image is transparent
    If the background color is also transparent, the image itself is stored transparently.
### 7. Let's save the image.
<img src="/Guide images/10.png"/>
    
    I modified the image like this. Now it's your turn to save.
    Click the Save button at the bottom right to save the image.
<img src="/Guide images/11.png"/>
    
    Saved!
### 8. Let's change the number of images per line.
    You can set whether to create an image that has several columns per row in the box next to the image.

    If the number of imported images is less than the size of the column you set, it is automatically set to the number of images.
<img src="/Guide images/12.png"/>

    There are currently 3 images, so I'll change the number to 2 and save it.
<img src="/Guide images/13.png"/>

    Two sheets saved per line. It worked out as intended.
