﻿using System;
using System.ComponentModel;
using System.Diagnostics;

namespace TestApp.My
{
    internal static partial class MyProject
    {
        internal partial class MyForms
        {

            [EditorBrowsable(EditorBrowsableState.Never)]
            public frmMain m_frmMain;

            public frmMain frmMain
            {
                [DebuggerHidden]
                get
                {
                    m_frmMain = Create__Instance__(m_frmMain);
                    return m_frmMain;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_frmMain))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_frmMain);
                }
            }

        }


    }
}