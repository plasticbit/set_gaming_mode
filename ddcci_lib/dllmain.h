#pragma once

extern "C"
{
    __declspec(dllexport) const HMONITOR __stdcall GetMonitor(HWND);
    __declspec(dllexport) const DWORD __stdcall GetNumMonitors(HMONITOR);
    __declspec(dllexport) const LPPHYSICAL_MONITOR __stdcall AllocMonitorStruct(HMONITOR, DWORD);
    __declspec(dllexport) const BOOL __stdcall GetPhysicalMonitor(HMONITOR, DWORD, LPPHYSICAL_MONITOR);
    __declspec(dllexport) const WCHAR* __stdcall GetDescriptionFromStruct(LPPHYSICAL_MONITOR, const int);
    __declspec(dllexport) const BOOL __stdcall SetVCPFeatureToMonitor(LPPHYSICAL_MONITOR, BYTE, DWORD);
    __declspec(dllexport) const BOOL __stdcall GetVCPFeatureCurrentValueFromMonitor(LPPHYSICAL_MONITOR, const int, BYTE, DWORD*);
    __declspec(dllexport) const BOOL __stdcall DestroyMonitorStruct(DWORD, LPPHYSICAL_MONITOR);
}