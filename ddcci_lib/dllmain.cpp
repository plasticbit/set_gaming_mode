#include "pch.h"

#pragma comment(lib, "Dxva2.lib")

using namespace std;

// use a pointer and an out operator
const HMONITOR __stdcall GetMonitor(HWND hwnd) {
    HMONITOR monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONULL);
    if (monitor == NULL) return NULL;

    return monitor;
}

const DWORD __stdcall GetNumMonitors(HMONITOR monitor) {
    DWORD size;
    if (!GetNumberOfPhysicalMonitorsFromHMONITOR(monitor, &size)) return NULL;

    return size;
}

const LPPHYSICAL_MONITOR __stdcall AllocMonitorStruct(HMONITOR monitor, DWORD size) {
    LPPHYSICAL_MONITOR monitorStruct = new PHYSICAL_MONITOR[size * sizeof(PHYSICAL_MONITOR)];
    if (monitorStruct == NULL) return NULL;

    return monitorStruct;
}

const BOOL __stdcall GetPhysicalMonitor(HMONITOR monitor, DWORD size, LPPHYSICAL_MONITOR lppms) {
    return GetPhysicalMonitorsFromHMONITOR(monitor, size, lppms);
}

const WCHAR* __stdcall GetDescriptionFromStruct(LPPHYSICAL_MONITOR lppms, const int idx) {
    return lppms[idx].szPhysicalMonitorDescription;
}

const BOOL __stdcall SetVCPFeatureToMonitor(LPPHYSICAL_MONITOR lppms, BYTE key, DWORD value) {
    return SetVCPFeature(lppms[0].hPhysicalMonitor, key, value);
}

const BOOL __stdcall GetVCPFeatureCurrentValueFromMonitor(LPPHYSICAL_MONITOR lppms, const int idx, BYTE code, DWORD* currentValue) {
    MC_VCP_CODE_TYPE  vcpCodeType;
    DWORD cv, maximumValue;

    return GetVCPFeatureAndVCPFeatureReply(lppms[idx].hPhysicalMonitor, code, &vcpCodeType, currentValue, &maximumValue);
}

const BOOL __stdcall DestroyMonitorStruct(DWORD size, LPPHYSICAL_MONITOR lppms) {
    return DestroyPhysicalMonitors(size, lppms);
}