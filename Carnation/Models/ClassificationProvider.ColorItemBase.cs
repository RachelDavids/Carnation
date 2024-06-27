﻿using System.Diagnostics.Contracts;
using System.Windows.Media;

namespace Carnation
{
    internal sealed partial class ClassificationProvider
    {
        internal abstract class ColorItemBase
            : NotifyPropertyBase
        {
            private uint _foregroundColorRef;
            public uint ForegroundColorRef
            {
                get => _foregroundColorRef;
                set
                {
                    _foregroundColorRef = value;
                    NotifyPropertyChanged(nameof(Foreground));
                }
            }

            public Color Foreground
            {
                get => IsForegroundEditable
                    ? FontsAndColorsHelper.TryGetColor(ForegroundColorRef) ?? DefaultForeground
                    : Colors.Transparent;
                set
                {
                    Contract.Assert(IsUpdating || IsForegroundEditable);
                    ForegroundColorRef = FontsAndColorsHelper.GetColorRef(value, DefaultForeground);
                }
            }

            private uint _autoForegroundColorRef;
            public uint AutoForegroundColorRef
            {
                get => _autoForegroundColorRef;
                set
                {
                    _autoForegroundColorRef = value;
                    NotifyPropertyChanged(nameof(DefaultForeground));
                }
            }

            public Color DefaultForeground => FontsAndColorsHelper.TryGetColor(AutoForegroundColorRef) ?? PlainTextForeground;

            private bool _isForegroundEditable = true;
            public bool IsForegroundEditable
            {
                get => _isForegroundEditable;
                set => SetProperty(ref _isForegroundEditable, value);
            }

            private uint _backgroundColorRef;
            public uint BackgroundColorRef
            {
                get => _backgroundColorRef;
                set
                {
                    _backgroundColorRef = value;
                    NotifyPropertyChanged(nameof(Background));
                }
            }

            public Color Background
            {
                get => IsBackgroundEditable
                    ? FontsAndColorsHelper.TryGetColor(BackgroundColorRef) ?? DefaultBackground
                    : Colors.Transparent;
                set
                {
                    Contract.Assert(IsUpdating || IsBackgroundEditable);
                    BackgroundColorRef = FontsAndColorsHelper.GetColorRef(value, DefaultBackground);
                }
            }

            private uint _autoBackgroundColorRef;
            public uint AutoBackgroundColorRef
            {
                get => _autoBackgroundColorRef;
                set
                {
                    _autoBackgroundColorRef = value;
                    NotifyPropertyChanged(nameof(DefaultBackground));
                }
            }

            public Color DefaultBackground => FontsAndColorsHelper.TryGetColor(AutoBackgroundColorRef) ?? PlainTextBackground;

            private bool _isBackgroundEditable = true;
            public bool IsBackgroundEditable
            {
                get => _isBackgroundEditable;
                set => SetProperty(ref _isBackgroundEditable, value);
            }

            private bool _isBold;
            public bool IsBold
            {
                get => _isBold;
                set
                {
                    Contract.Assert(IsUpdating || IsBoldEditable);
                    SetProperty(ref _isBold, value);
                }
            }

            private bool _isBoldEditable = true;
            public bool IsBoldEditable
            {
                get => _isBoldEditable;
                set => SetProperty(ref _isBoldEditable, value);
            }

            private double _contrastRatio;
            public double ContrastRatio
            {
                get => _contrastRatio;
                set => SetProperty(ref _contrastRatio, value);
            }

            protected ColorItemBase(
                uint foregroundColorRef,
                uint backgroundColorRef,
                uint autoForegroundColorRef,
                uint autoBackgroundColorRef,
                bool isBold,
                bool isForegroundEditable,
                bool isBackgroundEditable,
                bool isBoldEditable)
            {
                _foregroundColorRef = foregroundColorRef;
                _backgroundColorRef = backgroundColorRef;
                _autoForegroundColorRef = autoForegroundColorRef;
                _autoBackgroundColorRef = autoBackgroundColorRef;
                _isBold = isBold;
                _isForegroundEditable = isForegroundEditable;
                _isBackgroundEditable = isBackgroundEditable;
                _isBoldEditable = isBoldEditable;

                ComputeContrastRatio();

                PropertyChanged += (s, o) =>
                {
                    switch (o.PropertyName)
                    {
                        case nameof(Foreground):
                        case nameof(Background):
                            ComputeContrastRatio();
                            break;
                    }
                };
            }

            private void ComputeContrastRatio()
            {
                if (!IsForegroundEditable || !IsBackgroundEditable)
                {
                    ContrastRatio = 0.0;
                    return;
                }

                var contrast = ColorHelpers.GetContrast(Foreground, Background);
                ContrastRatio = contrast;
            }
        }
    }
}
