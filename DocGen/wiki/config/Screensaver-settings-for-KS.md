## Screensaver config entries

Generally, almost all screensavers have the color and delay settings. However, some have no such setting. Refer to the table below. Choose a section to go to extra screensaver settings.

| Name | Type | Values | Description 
|:--------------------------|:----------|:----------------------------------|:---
| Activate 255 Color Mode   | `boolean` | `true` or `false`                 | Activates the 255 color mode. Overridden by the true color mode.
| Activate True Color Mode  | `boolean` | `true` or `false`                 | Activates the 255 color mode. Overridden by the true color mode.
| Background color          | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the background color
| Foreground color          | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the foreground color
| Delay in Milliseconds     | `integer` | Interval in milliseconds          | Specify when will the screensaver wait before the next write.
| Minimum Red Color Level   | `integer` | From 0 to 255                     | Specify the minimum red color level (used for generating random RGB color)
| Minimum Green Color Level | `integer` | From 0 to 255                     | Specify the minimum green color level (used for generating random RGB color)
| Minimum Blue Color Level  | `integer` | From 0 to 255                     | Specify the minimum blue color level (used for generating random RGB color)
| Minimum Color Level       | `integer` | From 0 to 255                     | Specify the minimum color level (used for generating normal color)
| Maximum Red Color Level   | `integer` | From 0 to 255                     | Specify the maximum red color level (used for generating random RGB color)
| Maximum Green Color Level | `integer` | From 0 to 255                     | Specify the maximum green color level (used for generating random RGB color)
| Maximum Blue Color Level  | `integer` | From 0 to 255                     | Specify the maximum blue color level (used for generating random RGB color)
| Maximum Color Level       | `integer` | From 0 to 255                     | Specify the maximum color level (used for generating normal color)

### Disco

| Name | Type | Values | Description 
|:----------------------------|:----------|:------------------|:---
| Cycle colors                | `boolean` | `true` or `false` | Specifies whether to cycle the colors
| Use BPM                     | `boolean` | `true` or `false` | Specifies whether to use Beats per Minute for delays
| Enable Black and White Mode | `boolean` | `true` or `false` | Enables the fed-like black and white mode. This really hurts your eyes. Don't enable if you have photosensitivity seizure.

### Lines

| Name | Type | Values | Description 
|:-----------------|:---------|:----------------------------------|:---
| Line character   | `char`   | A valid character                 | Which character to use to draw the line?

### BouncingText

| Name | Type | Values | Description 
|:-----------|:---------|:-------|:---
| Text Shown | `string` | A text | The text that is shown. If the text is longer than console width, it will be truncated by 15 characters.

### ProgressClock

| Name | Type | Values | Description 
|:--------------------------------|:----------|:----------------------------------|:---
| Cycle colors                    | `boolean` | `true` or `false`                 | Specifies whether to cycle the colors
| Seconds Progress Color          | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | The progress color for the seconds bar
| Minutes Progress Color          | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | The progress color for the minutes bar
| Hours Progress Color            | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | The progress color for the hours bar
| Progress Color                  | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | The progress color
| Cycle Colors Ticks              | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | How many ticks to cycle the color?
| Upper Left Corner Char Hours    | `char`    | A valid character                 | Which character to use to draw the upper left corner of the hours bar?
| Upper Left Corner Char Minutes  | `char`    | A valid character                 | Which character to use to draw the upper left corner of the minutes bar?
| Upper Left Corner Char Seconds  | `char`    | A valid character                 | Which character to use to draw the upper left corner of the seconds bar?
| Upper Right Corner Char Hours   | `char`    | A valid character                 | Which character to use to draw the upper right corner of the hours bar?
| Upper Right Corner Char Minutes | `char`    | A valid character                 | Which character to use to draw the upper right corner of the minutes bar?
| Upper Right Corner Char Seconds | `char`    | A valid character                 | Which character to use to draw the upper right corner of the seconds bar?
| Lower Left Corner Char Hours    | `char`    | A valid character                 | Which character to use to draw the lower left corner of the hours bar?
| Lower Left Corner Char Minutes  | `char`    | A valid character                 | Which character to use to draw the lower left corner of the minutes bar?
| Lower Left Corner Char Seconds  | `char`    | A valid character                 | Which character to use to draw the lower left corner of the seconds bar?
| Lower Right Corner Char Hours   | `char`    | A valid character                 | Which character to use to draw the lower right corner of the hours bar?
| Lower Right Corner Char Minutes | `char`    | A valid character                 | Which character to use to draw the lower right corner of the minutes bar? 
| Lower Right Corner Char Seconds | `char`    | A valid character                 | Which character to use to draw the lower right corner of the seconds bar?
| Upper Frame Char Hours          | `char`    | A valid character                 | Which character to use to draw the upper frame of the hours bar?
| Upper Frame Char Minutes        | `char`    | A valid character                 | Which character to use to draw the upper frame of the minutes bar?
| Upper Frame Char Seconds        | `char`    | A valid character                 | Which character to use to draw the upper frame of the seconds bar?
| Lower Frame Char Hours          | `char`    | A valid character                 | Which character to use to draw the lower frame of the hours bar?
| Lower Frame Char Minutes        | `char`    | A valid character                 | Which character to use to draw the lower frame of the minutes bar?
| Lower Frame Char Seconds        | `char`    | A valid character                 | Which character to use to draw the lower frame of the seconds bar?
| Left Frame Char Hours           | `char`    | A valid character                 | Which character to use to draw the left frame of the hours bar?
| Left Frame Char Minutes         | `char`    | A valid character                 | Which character to use to draw the left frame of the minutes bar?
| Left Frame Char Seconds         | `char`    | A valid character                 | Which character to use to draw the left frame of the seconds bar?
| Right Frame Char Hours          | `char`    | A valid character                 | Which character to use to draw the right frame of the hours bar?
| Right Frame Char Minutes        | `char`    | A valid character                 | Which character to use to draw the right frame of the minutes bar?
| Right Frame Char Seconds        | `char`    | A valid character                 | Which character to use to draw the right frame of the seconds bar?
| Info Text Hours                 | `string`  | A text string                     | What text to enter in the hours part for info?
| Info Text Minutes               | `string`  | A text string                     | What text to enter in the minutes part for info?
| Info Text Seconds               | `string`  | A text string                     | What text to enter in the seconds part for info?

For the progress min-max color values, refer to the topmost table, because they're alike functionally, but focus on different progresses.

### Lighter

| Name | Type | Values | Description 
|:--------------------|:----------|:--------------------|:---
| Max Positions Count | `integer` | Any positive number | How many positions are lit before dimming? If the block count exceeded this number, the earliest block that appeared will disappear.

### Wipe

| Name | Type | Values | Description 
|:--------------------------|:----------|:--------------------|:---
| Wipes to change direction | `integer` | Any positive number | How many wipes to do before changing direction randomly?

### Marquee

| Name | Type | Values | Description 
|:----------------|:----------|:------------------|:---
| Text Shown      | `string`  | A text            | The text that is shown.
| Always centered | `boolean` | `true` or `false` | Whether the text shown is always on the middle
| Use Console API | `boolean` | `true` or `false` | Whether to use the standard Console.Clear() or to use the VT sequence.

### BeatFader

| Name | Type | Values | Description 
|:-------------|:----------|:----------------------------------|:---
| Cycle colors | `boolean` | `true` or `false`                 | Specifies whether to cycle the colors
| Beat color   | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the beat color
| Max steps    | `integer` | Any positive number               | How many fade steps to do?

### Fader

| Name | Type | Values | Description 
|:-------------------------------|:----------|:-------------------------|:---
| Text Shown                     | `string`  | A text                   | The text that is shown.
| Fade Out Delay in Milliseconds | `integer` | Interval in milliseconds | How many milliseconds to wait before fading out text?
| Max Fade Steps                 | `integer` | Any positive number      | How many fade steps to do?

### FaderBack

| Name | Type | Values | Description 
|:-------------------------------|:----------|:-------------------------|:---
| Fade Out Delay in Milliseconds | `integer` | Interval in milliseconds | How many milliseconds to wait before fading out text?
| Max Fade Steps                 | `integer` | Any positive number      | How many fade steps to do?

### Typo

| Name | Type | Values | Description 
|:----------------------------------|:----------|:----------------------------------|:---
| Write Again Delay in Milliseconds | `integer` | Interval in milliseconds          | How many milliseconds to wait before writing text again?
| Text Shown                        | `string`  | A text                            | The text that is shown.
| Minimum writing speed in WPM      | `integer` | Speed in WPM                      | Self-explanatory
| Maximum writing speed in WPM      | `integer` | Speed in WPM                      | Self-explanatory
| Probability of typo in percent    | `integer` | Percent value from 0 to 100       | Self-explanatory
| Probability of miss in percent    | `integer` | Percent value from 0 to 100       | Self-explanatory
| Text color                        | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the text color

### Linotypo

| Name | Type | Values | Description 
|:----------------------------------|:----------|:----------------------------------|:---
| New Screen Delay in Milliseconds  | `integer` | Interval in milliseconds          | How many milliseconds to wait before writing text again?
| Text Shown                        | `string`  | A text                            | The text that is shown.
| Minimum writing speed in WPM      | `integer` | Speed in WPM                      | Self-explanatory
| Maximum writing speed in WPM      | `integer` | Speed in WPM                      | Self-explanatory
| Probability of typo in percent    | `integer` | Percent value from 0 to 100       | Self-explanatory
| Probability of miss in percent    | `integer` | Percent value from 0 to 100       | Self-explanatory
| Text Columns                      | `integer` | Columns from 1 to 3               | How many columns to print?
| Etaoin Threshold                  | `integer` | Number of characters              | How many number of characters to count before writing the Etaoin pattern?
| Etaoin Capping Possibility        | `integer` | Percent value from 0 to 100       | Self-explanatory
| Etaoin Type                       | `string`  | FillType value string             | Chooses the type of Etaoin
| Text color                        | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the text color

### Typewriter

| Name | Type | Values | Description 
|:----------------------------------|:----------|:----------------------------------|:---
| Write Again Delay in Milliseconds | `integer` | Interval in milliseconds          | How many milliseconds to wait before writing text again?
| Text Shown                        | `string`  | A text                            | The text that is shown.
| Minimum writing speed in WPM      | `integer` | Speed in WPM                      | Self-explanatory
| Maximum writing speed in WPM      | `integer` | Speed in WPM                      | Self-explanatory
| Show arrow position               | `boolean` | `true` or `false`                 | Shows the arrow position
| Text color                        | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the text color

### SpotWrite

| Name | Type | Values | Description 
|:----------------------------------|:----------|:----------------------------------|:---
| Write Again Delay in Milliseconds | `integer` | Interval in milliseconds          | How many milliseconds to wait before writing text again?
| Text Shown                        | `string`  | A text                            | The text that is shown.
| Text color                        | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the text color

### Ramp

| Name | Type | Values | Description 
|:-------------------------|:----------|:----------------------------------|:---
| Next ramp delay          | `integer` | Interval in milliseconds          | How many milliseconds to wait before filling the next ramp?
| Upper Left Corner Char   | `char`    | A valid character                 | Which character to use to draw the upper left corner of the ramp?
| Upper Right Corner Char  | `char`    | A valid character                 | Which character to use to draw the upper right corner of the ramp?
| Lower Left Corner Char   | `char`    | A valid character                 | Which character to use to draw the lower left corner of the ramp?
| Lower Right Corner Char  | `char`    | A valid character                 | Which character to use to draw the lower right corner of the ramp?
| Upper Frame Char         | `char`    | A valid character                 | Which character to use to draw the upper frame of the ramp?
| Lower Frame Char         | `char`    | A valid character                 | Which character to use to draw the lower frame of the ramp?
| Left Frame Char          | `char`    | A valid character                 | Which character to use to draw the left frame of the ramp?
| Right Frame Char         | `char`    | A valid character                 | Which character to use to draw the right frame of the ramp?
| Upper Left Corner Color  | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the upper left corner of the ramp
| Upper Right Corner Color | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the upper right corner of the ramp
| Lower Left Corner Color  | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the lower left corner of the ramp
| Lower Right Corner Color | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the lower right corner of the ramp
| Upper Frame Color        | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the upper frame of the ramp
| Lower Frame Color        | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the lower frame of the ramp
| Left Frame Color         | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the left frame of the ramp
| Right Frame Color        | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the right frame of the ramp
| Use Border Colors        | `boolean` | `true` or `false`                 | Uses the border colors

### StackBox

| Name | Type | Values | Description 
|:---------------|:----------|:------------------|:---
| Fill the boxes | `boolean` | `true` or `false` | Selects whether to fill in the boxes or just draw the borders.

### Snaker

| Name | Type | Values | Description 
|:-----------------|:----------|:-------------------------|:---
| Next stage delay | `integer` | Interval in milliseconds | How many milliseconds to wait before making the next stage?

### BarRot

| Name | Type | Values | Description 
|:-------------------------|:----------|:----------------------------------|:---
| Next ramp delay          | `integer` | Interval in milliseconds          | How many milliseconds to wait before rotting the next ramp?
| Upper Left Corner Char   | `char`    | A valid character                 | Which character to use to draw the upper left corner of the ramp?
| Upper Right Corner Char  | `char`    | A valid character                 | Which character to use to draw the upper right corner of the ramp?
| Lower Left Corner Char   | `char`    | A valid character                 | Which character to use to draw the lower left corner of the ramp?
| Lower Right Corner Char  | `char`    | A valid character                 | Which character to use to draw the lower right corner of the ramp?
| Upper Frame Char         | `char`    | A valid character                 | Which character to use to draw the upper frame of the ramp?
| Lower Frame Char         | `char`    | A valid character                 | Which character to use to draw the lower frame of the ramp?
| Left Frame Char          | `char`    | A valid character                 | Which character to use to draw the left frame of the ramp?
| Right Frame Char         | `char`    | A valid character                 | Which character to use to draw the right frame of the ramp?
| Upper Left Corner Color  | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the upper left corner of the ramp
| Upper Right Corner Color | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the upper right corner of the ramp
| Lower Left Corner Color  | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the lower left corner of the ramp
| Lower Right Corner Color | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the lower right corner of the ramp
| Upper Frame Color        | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the upper frame of the ramp
| Lower Frame Color        | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the lower frame of the ramp
| Left Frame Color         | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the left frame of the ramp
| Right Frame Color        | `string`  | `0-15`, `0-255`, or `RRR;GGG;BBB` | Selects the color of the right frame of the ramp
| Use Border Colors        | `boolean` | `true` or `false`                 | Uses the border colors

### Fireworks

| Name | Type | Values | Description 
|:-----------------|:----------|:-----------------|:---
| Explosion Radius | `integer` | Radius in blocks | The radius of the explosion by blocks

### Figlet

| Name | Type | Values | Description 
|:------------|:---------|:-------------------------|:---
| Figlet font | `string` | Font supported by Figgle | The font to use. It should be supported by Figgle.
| Text Shown  | `string` | A text                   | The text that is shown.


### Noise

| Name | Type | Values | Description 
|:----------------------------------|:----------|:----------------------------------|:---
| New Screen Delay in Milliseconds  | `integer` | Interval in milliseconds          | How many milliseconds to wait before writing text again?
| Noise Density                     | `integer` | Percent value from 0 to 100       | The greater the percentage, the noisier the signal.

### FlashText

| Name | Type | Values | Description 
|:-----------|:---------|:-------|:---
| Text Shown | `string` | A text | The text that is shown.
