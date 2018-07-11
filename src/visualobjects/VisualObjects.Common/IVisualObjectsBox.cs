﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace VisualObjects.Common
{
    public interface IVisualObjectsBox
    {
        string GetJson();

        void SetObject(string objectId, string objectJson);
    }
}
