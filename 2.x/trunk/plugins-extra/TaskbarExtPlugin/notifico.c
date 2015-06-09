/*
 * Process Hacker Extra Plugins -
 *   Taskbar Extensions
 *
 * Copyright (C) 2015 dmex
 * Copyright (C) 2011-2013 wj32
 *
 * This file is part of Process Hacker.
 *
 * Process Hacker is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Process Hacker is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Process Hacker.  If not, see <http://www.gnu.org/licenses/>.
 */

#include "main.h"

typedef struct _PH_NF_BITMAP
{
    BOOLEAN Initialized;
    HDC Hdc;
    BITMAPINFOHEADER Header;
    HBITMAP Bitmap;
    PVOID Bits;
} PH_NF_BITMAP, *PPH_NF_BITMAP;

static PH_NF_BITMAP PhDefaultBitmapContext = { 0 };
static PH_NF_BITMAP PhBlackBitmapContext = { 0 };
static HBITMAP PhBlackBitmap = NULL;
static HICON PhBlackIcon = NULL;

static VOID PhBeginBitmap2(
    _Inout_ PPH_NF_BITMAP Context,
    _Out_ PULONG Width,
    _Out_ PULONG Height,
    _Out_ HBITMAP *Bitmap,
    _Out_opt_ PVOID *Bits,
    _Out_ HDC *Hdc,
    _Out_ HBITMAP *OldBitmap
    )
{
    if (!Context->Initialized)
    {
        HDC hdc;

        hdc = GetDC(PhMainWndHandle);
        Context->Hdc = CreateCompatibleDC(hdc);

        memset(&Context->Header, 0, sizeof(BITMAPINFOHEADER));
        Context->Header.biSize = sizeof(BITMAPINFOHEADER);
        Context->Header.biWidth = 16;
        Context->Header.biHeight = 16;
        Context->Header.biPlanes = 1;
        Context->Header.biBitCount = 32;
        Context->Bitmap = CreateDIBSection(hdc, (BITMAPINFO*)&Context->Header, DIB_RGB_COLORS, &Context->Bits, NULL, 0);

        ReleaseDC(PhMainWndHandle, hdc);

        Context->Initialized = TRUE;
    }

    *Width = 16;
    *Height = 16;
    *Bitmap = Context->Bitmap;
    if (Bits) *Bits = Context->Bits;
    *Hdc = Context->Hdc;
    *OldBitmap = SelectObject(Context->Hdc, Context->Bitmap);
}

static VOID PhBeginBitmap(
    _Out_ PULONG Width,
    _Out_ PULONG Height,
    _Out_ HBITMAP *Bitmap,
    _Out_opt_ PVOID *Bits,
    _Out_ HDC *Hdc,
    _Out_ HBITMAP *OldBitmap
    )
{
    PhBeginBitmap2(&PhDefaultBitmapContext, Width, Height, Bitmap, Bits, Hdc, OldBitmap);
}

static HICON PhBitmapToIcon(
    _In_ HBITMAP Bitmap
    )
{
    ICONINFO iconInfo;

    PhGetBlackIcon();

    iconInfo.fIcon = TRUE;
    iconInfo.xHotspot = 0;
    iconInfo.yHotspot = 0;
    iconInfo.hbmMask = PhBlackBitmap;
    iconInfo.hbmColor = Bitmap;

    return CreateIconIndirect(&iconInfo);
}

HICON PhGetBlackIcon(
    VOID
    )
{
    if (!PhBlackIcon)
    {
        ULONG width;
        ULONG height;
        PVOID bits;
        HDC hdc;
        HBITMAP oldBitmap;
        ICONINFO iconInfo;

        PhBeginBitmap2(&PhBlackBitmapContext, &width, &height, &PhBlackBitmap, &bits, &hdc, &oldBitmap);
        memset(bits, 0, width * height * sizeof(ULONG));

        iconInfo.fIcon = TRUE;
        iconInfo.xHotspot = 0;
        iconInfo.yHotspot = 0;
        iconInfo.hbmMask = PhBlackBitmap;
        iconInfo.hbmColor = PhBlackBitmap;
        PhBlackIcon = CreateIconIndirect(&iconInfo);

        SelectObject(hdc, oldBitmap);
    }

    return PhBlackIcon;
}

HICON PhUpdateIconCpuHistory(
    _In_ PH_PLUGIN_SYSTEM_STATISTICS Statistics
    )
{
    static PH_GRAPH_DRAW_INFO drawInfo =
    {
        16,
        16,
        PH_GRAPH_USE_LINE_2,
        2,
        RGB(0x00, 0x00, 0x00),
        16,
        NULL,
        NULL,
        0,
        0,
        0,
        0
    };
    ULONG maxDataCount;
    ULONG lineDataCount;
    PFLOAT lineData1;
    PFLOAT lineData2;
    HBITMAP bitmap;
    PVOID bits;
    HDC hdc;
    HBITMAP oldBitmap;
    HICON icon;

    // Icon
    PhBeginBitmap(&drawInfo.Width, &drawInfo.Height, &bitmap, &bits, &hdc, &oldBitmap);
    maxDataCount = drawInfo.Width / 2 + 1;
    lineData1 = _alloca(maxDataCount * sizeof(FLOAT));
    lineData2 = _alloca(maxDataCount * sizeof(FLOAT));

    lineDataCount = min(maxDataCount, Statistics.CpuKernelHistory->Count);
    PhCopyCircularBuffer_FLOAT(Statistics.CpuKernelHistory, lineData1, lineDataCount);
    PhCopyCircularBuffer_FLOAT(Statistics.CpuUserHistory, lineData2, lineDataCount);

    drawInfo.LineDataCount = lineDataCount;
    drawInfo.LineData1 = lineData1;
    drawInfo.LineData2 = lineData2;
    drawInfo.LineColor1 = PhGetIntegerSetting(L"ColorCpuKernel");
    drawInfo.LineColor2 = PhGetIntegerSetting(L"ColorCpuUser");
    drawInfo.LineBackColor1 = PhHalveColorBrightness(drawInfo.LineColor1);
    drawInfo.LineBackColor2 = PhHalveColorBrightness(drawInfo.LineColor2);

    if (bits)
        PhDrawGraphDirect(hdc, bits, &drawInfo);

    SelectObject(hdc, oldBitmap);
    icon = PhBitmapToIcon(bitmap);

    return icon;
}

HICON PhUpdateIconIoHistory(
    _In_ PH_PLUGIN_SYSTEM_STATISTICS Statistics
    )
{
    static PH_GRAPH_DRAW_INFO drawInfo =
    {
        16,
        16,
        PH_GRAPH_USE_LINE_2,
        2,
        RGB(0x00, 0x00, 0x00),
        16,
        NULL,
        NULL,
        0,
        0,
        0,
        0
    };
    ULONG maxDataCount;
    ULONG lineDataCount;
    PFLOAT lineData1;
    PFLOAT lineData2;
    FLOAT max;
    ULONG i;
    HBITMAP bitmap;
    PVOID bits;
    HDC hdc;
    HBITMAP oldBitmap;
    HICON icon;

    // Icon
    PhBeginBitmap(&drawInfo.Width, &drawInfo.Height, &bitmap, &bits, &hdc, &oldBitmap);
    maxDataCount = drawInfo.Width / 2 + 1;
    lineData1 = _alloca(maxDataCount * sizeof(FLOAT));
    lineData2 = _alloca(maxDataCount * sizeof(FLOAT));

    lineDataCount = min(maxDataCount, Statistics.IoReadHistory->Count);
    max = 1024 * 1024; // minimum scaling of 1 MB.

    for (i = 0; i < lineDataCount; i++)
    {
        lineData1[i] = (FLOAT)PhGetItemCircularBuffer_ULONG64(Statistics.IoReadHistory, i) 
                     + (FLOAT)PhGetItemCircularBuffer_ULONG64(Statistics.IoOtherHistory, i);
        lineData2[i] = (FLOAT)PhGetItemCircularBuffer_ULONG64(Statistics.IoWriteHistory, i);

        if (max < lineData1[i] + lineData2[i])
            max = lineData1[i] + lineData2[i];
    }

    PhDivideSinglesBySingle(lineData1, max, lineDataCount);
    PhDivideSinglesBySingle(lineData2, max, lineDataCount);

    drawInfo.LineDataCount = lineDataCount;
    drawInfo.LineData1 = lineData1;
    drawInfo.LineData2 = lineData2;
    drawInfo.LineColor1 = PhGetIntegerSetting(L"ColorIoReadOther");
    drawInfo.LineColor2 = PhGetIntegerSetting(L"ColorIoWrite");
    drawInfo.LineBackColor1 = PhHalveColorBrightness(drawInfo.LineColor1);
    drawInfo.LineBackColor2 = PhHalveColorBrightness(drawInfo.LineColor2);

    if (bits)
        PhDrawGraphDirect(hdc, bits, &drawInfo);

    SelectObject(hdc, oldBitmap);
    icon = PhBitmapToIcon(bitmap);

    return icon;
}

HICON PhUpdateIconCommitHistory(
    _In_ PH_PLUGIN_SYSTEM_STATISTICS Statistics
    )
{
    static PH_GRAPH_DRAW_INFO drawInfo =
    {
        16,
        16,
        0,
        2,
        RGB(0x00, 0x00, 0x00),
        16,
        NULL,
        NULL,
        0,
        0,
        0,
        0
    };
    ULONG maxDataCount;
    ULONG lineDataCount;
    PFLOAT lineData1;
    ULONG i;
    HBITMAP bitmap;
    PVOID bits;
    HDC hdc;
    HBITMAP oldBitmap;
    HICON icon;

    // Icon
    PhBeginBitmap(&drawInfo.Width, &drawInfo.Height, &bitmap, &bits, &hdc, &oldBitmap);
    maxDataCount = drawInfo.Width / 2 + 1;
    lineData1 = _alloca(maxDataCount * sizeof(FLOAT));

    lineDataCount = min(maxDataCount, Statistics.CommitHistory->Count);

    for (i = 0; i < lineDataCount; i++)
        lineData1[i] = (FLOAT)PhGetItemCircularBuffer_ULONG(Statistics.CommitHistory, i);

    PhDivideSinglesBySingle(lineData1, (FLOAT)Statistics.Performance->CommitLimit, lineDataCount);

    drawInfo.LineDataCount = lineDataCount;
    drawInfo.LineData1 = lineData1;
    drawInfo.LineColor1 = PhGetIntegerSetting(L"ColorPrivate");
    drawInfo.LineBackColor1 = PhHalveColorBrightness(drawInfo.LineColor1);

    if (bits)
        PhDrawGraphDirect(hdc, bits, &drawInfo);

    SelectObject(hdc, oldBitmap);
    icon = PhBitmapToIcon(bitmap);

    return icon;
}

HICON PhUpdateIconPhysicalHistory(
    _In_ PH_PLUGIN_SYSTEM_STATISTICS Statistics
    )
{
    static PH_GRAPH_DRAW_INFO drawInfo =
    {
        16,
        16,
        0,
        2,
        RGB(0x00, 0x00, 0x00),
        16,
        NULL,
        NULL,
        0,
        0,
        0,
        0
    };
    ULONG maxDataCount;
    ULONG lineDataCount;
    PFLOAT lineData1;
    ULONG i;
    HBITMAP bitmap;
    PVOID bits;
    HDC hdc;
    HBITMAP oldBitmap;
    HICON icon;

    // Icon
    PhBeginBitmap(&drawInfo.Width, &drawInfo.Height, &bitmap, &bits, &hdc, &oldBitmap);
    maxDataCount = drawInfo.Width / 2 + 1;
    lineData1 = _alloca(maxDataCount * sizeof(FLOAT));

    lineDataCount = min(maxDataCount, Statistics.CommitHistory->Count);

    for (i = 0; i < lineDataCount; i++)
        lineData1[i] = (FLOAT)PhGetItemCircularBuffer_ULONG(Statistics.PhysicalHistory, i);

    PhDivideSinglesBySingle(lineData1, (FLOAT)PhSystemBasicInformation.NumberOfPhysicalPages, lineDataCount);

    drawInfo.LineDataCount = lineDataCount;
    drawInfo.LineData1 = lineData1;
    drawInfo.LineColor1 = PhGetIntegerSetting(L"ColorPhysical");
    drawInfo.LineBackColor1 = PhHalveColorBrightness(drawInfo.LineColor1);

    if (bits)
        PhDrawGraphDirect(hdc, bits, &drawInfo);

    SelectObject(hdc, oldBitmap);
    icon = PhBitmapToIcon(bitmap);

    return icon;
}

HICON PhUpdateIconCpuUsage(
    _In_ PH_PLUGIN_SYSTEM_STATISTICS Statistics
    )
{
    ULONG width;
    ULONG height;
    HBITMAP bitmap;
    HDC hdc;
    HBITMAP oldBitmap;
    HICON icon;

    // Icon
    PhBeginBitmap(&width, &height, &bitmap, NULL, &hdc, &oldBitmap);

    // This stuff is copied from CpuUsageIcon.cs (PH 1.x).
    {
        COLORREF kColor = PhGetIntegerSetting(L"ColorCpuKernel");
        COLORREF uColor = PhGetIntegerSetting(L"ColorCpuUser");
        COLORREF kbColor = PhHalveColorBrightness(kColor);
        COLORREF ubColor = PhHalveColorBrightness(uColor);
        FLOAT k = Statistics.CpuKernelUsage;
        FLOAT u = Statistics.CpuUserUsage;
        LONG kl = (LONG)(k * height);
        LONG ul = (LONG)(u * height);
        RECT rect;
        HBRUSH dcBrush;
        HBRUSH dcPen;
        POINT points[2];

        dcBrush = GetStockObject(DC_BRUSH);
        dcPen = GetStockObject(DC_PEN);
        rect.left = 0;
        rect.top = 0;
        rect.right = width;
        rect.bottom = height;
        SetDCBrushColor(hdc, RGB(0x00, 0x00, 0x00));
        FillRect(hdc, &rect, dcBrush);

        // Draw the base line.
        if (kl + ul == 0)
        {
            SelectObject(hdc, dcPen);
            SetDCPenColor(hdc, uColor);
            points[0].x = 0;
            points[0].y = height - 1;
            points[1].x = width;
            points[1].y = height - 1;
            Polyline(hdc, points, 2);
        }
        else
        {
            rect.left = 0;
            rect.top = height - ul - kl;
            rect.right = width;
            rect.bottom = height - kl;
            SetDCBrushColor(hdc, ubColor);
            FillRect(hdc, &rect, dcBrush);

            points[0].x = 0;
            points[0].y = height - 1 - ul - kl;
            if (points[0].y < 0) points[0].y = 0;
            points[1].x = width;
            points[1].y = points[0].y;
            SelectObject(hdc, dcPen);
            SetDCPenColor(hdc, uColor);
            Polyline(hdc, points, 2);

            if (kl != 0)
            {
                rect.left = 0;
                rect.top = height - kl;
                rect.right = width;
                rect.bottom = height;
                SetDCBrushColor(hdc, kbColor);
                FillRect(hdc, &rect, dcBrush);

                points[0].x = 0;
                points[0].y = height - 1 - kl;
                if (points[0].y < 0) points[0].y = 0;
                points[1].x = width;
                points[1].y = points[0].y;
                SelectObject(hdc, dcPen);
                SetDCPenColor(hdc, kColor);
                Polyline(hdc, points, 2);
            }
        }
    }

    SelectObject(hdc, oldBitmap);
    icon = PhBitmapToIcon(bitmap);

    return icon;
}