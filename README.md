# Asus Fan Control

### Download
Go to [releases](../../releases).
Ensure you have the latest version.

# Help! My application is not working.
The most recent update to Asus System Control Interface (V3.1.41.0) has broken some Asus fan control softwares [#44 (comment)](https://github.com/Karmel0x/AsusFanControl/issues/44#issuecomment-2675958931). To restore functionality, you will need to downgrade to version (V3.1.38.0).

Steps:
1. Go to "Asus System Control Interface V3", under Device Manager -> System Devices -> Asus System Control Interface V3
2. Right click -> Properties -> Driver tab
3. Click "Roll Back Driver"
4. Restart your computer.

If "Roll Back Driver" is unavailable:
1. click Uninstall Device
2. Ensure "Attempt to remove the driver for this device." is ticked.
3. Reinstall an older version by going to [this page](https://github.com/Karmel0x/AsusFanControl/releases/) and installing driver "ASUSSystemControlInterfaceV3_3.1.38.0.exe".
4. Restart your computer.

Temporarily disable driver updates until a permanent fix is found:
0. If Roll Back Driver worked for you then no need to disable driver updates.
1. Open Powershell as an Administrator
2. Run command `REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate" /f /v ExcludeWUDriversInQualityUpdate /t REG_DWORD /d 1`

### Run

<details>
    <summary>Command line: `AsusFanControl.exe`</summary>
    
    AsusFanControl.exe <args>
        --get-fan-speeds
        --set-fan-speeds=0-100 (percent value, 0 for turning off test mode)
        --get-fan-count
        --get-fan-speed=fanId (comma separated)
        --set-fan-speed=fanId:0-100 (comma separated, percent value, 0 for turning off test mode)
        --get-cpu-temp
</details>

GUI: `AsusFanControlGUI.exe`  

![AsusFanControlGUI](https://github.com/Darren80/AsusFanControlEnhanced/blob/9bdf4b80158414a3258da22b16b35a36152eee8c/Screenshot%202024-05-20%20225642.png)

### Why need it?
My laptop does not support the [Fan Profile](https://github.com/Karmel0x/AsusFanControl/assets/25367564/924d990a-bf20-4b8d-bf9d-56c460174d99) option, but it often overheats. Looked for apps to control fans, but none is working.

### Compatibility
This program should work on any laptop with x64 windows where [Fan Diagnosis](https://github.com/Karmel0x/AsusFanControl/assets/25367564/7129833b-97af-4da8-9148-b71e49552ea4) in [MyASUS](https://apps.microsoft.com/store/detail/myasus/9N7R5S6B0ZZH) application is working as it is using same library.

Included `AsusWinIO64.dll` is licenced to `(c) ASUSTek COMPUTER INC.` which can be found in `C:\Windows\System32\DriverStore\FileRepository\asussci2.inf_amd64_-\ASUSSystemAnalysis\` if you have MyASUS installed.

Confirmed compatibility: 
- ASUS VivoBook 15 X512FL
- ASUS VivoBook 15 515JA
- ASUS VivoBook 16x M1603QA
- ASUS TUF Gaming FX705DY
- ASUS TUF Gaming A15 FX506IV
- ASUS TUF Gaming A15 FA506IC
- ASUS TUF Gaming FX505DV
- ASUS VivoBook 14 X412DA
- ASUS Zenbook Pro Duo 15 (UX582ZW)
- ASUS Vivobook Pro 16X (K6604)
- ASUS TUF Gaming FX505D
- ASUS TUF F15 - FX506HM
- ASUS ZenBook 14 UM425QA-KI174W
- ASUS ROG Strix G512LW

### Re-enable driver updates
`REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate" /f /v ExcludeWUDriversInQualityUpdate /t REG_DWORD /d 0`
