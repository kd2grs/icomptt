# icomptt
PTT program for ICOM radios, enables RTS signaling to key radio

This software allows ham radio programs that key the radio via RS-232 pin setting (e.g. assert RTS pin) to key up (active transmit) on an ICOM radio via CAT commands. This program is primarily used to allow [Direwolf](https://github.com/wb2osz/direwolf) and [UZ7HO](uz7.ho.ua/packetradio.htm) soundmodem TNC software to key up ICOM IC-7100, IC-7200 and IC-9100 radios. It is designed to run on Windows 7 or Windows 10 computers and it requires the .NET framework and [com0com](https://sourceforge.net/projects/com0com/) virtual NULL modem software to run.

## Background Info
Many radios with computer control via RS-232 utilitize the RS-232 pins to control the function of the radio. An application raising the RTS (ready to send) pin causes the radio to enter transmit mode. Less common but possible is that the radio can also utilize the CTS (clear to send) or CD (carrier detect) pins to signal the computer software of the presence of an incoming signal. This is in addition to normal serial data transmitted on the RX and TX pins which contain commands to control the radio - setting the frequency, operating modes (AM, FM, USB, LSB, etc...), filters and nearly all other settings available on the radio itself. Putting the radio into transmit mode or recieve mode are serial commands that can be send to the radio.

The ICOM IC-7100, IC-7200 and IC-9100 radios have a USB (universal serial bus) interface to a computer. When connected to a computer via a USB cable, with the proper drivers, the radio will be seen as an extra sound card on the computer and also two viurtual serial ports will be created and available.

## Installation and Configuration
