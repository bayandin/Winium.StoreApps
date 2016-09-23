# coding: utf-8
import os

BASE_DIR = os.path.dirname(os.path.dirname(__file__))
CONFIG_IDENTIFIER = '' if os.getenv('REMOTE_RUN') else '_Debug'
APPX_PATH = r"D:\Projects\windows-universal-app-driver\Winium\TestApp\TestApp.WindowsPhone\AppPackages\TestApp.WindowsPhone_1.0.0.0_AnyCPU_Debug_Test\TestApp.WindowsPhone_1.0.0.0_AnyCPU_Debug.appx"

DESIRED_CAPABILITIES = {
    "app": os.path.abspath(os.path.join(BASE_DIR, APPX_PATH)),
    "deviceName": "Emulator 8.1"
    # "debugConnectToRunningApp": True
}
