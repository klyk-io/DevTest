"use strict";
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
Object.defineProperty(exports, "__esModule", { value: true });
exports.AbortController = void 0;
// Rough polyfill of https://developer.mozilla.org/en-US/docs/Web/API/AbortController
// We don't actually ever use the API being polyfilled, we always use the polyfill because
// it's a very new API right now.
// Not exported from index.
/** @private */
var AbortController = /** @class */ (function () {
    function AbortController() {
        this._isAborted = false;
        this.onabort = null;
    }
    AbortController.prototype.abort = function () {
        if (!this._isAborted) {
            this._isAborted = true;
            if (this.onabort) {
                this.onabort();
            }
        }
    };
    Object.defineProperty(AbortController.prototype, "signal", {
        get: function () {
            return this;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(AbortController.prototype, "aborted", {
        get: function () {
            return this._isAborted;
        },
        enumerable: false,
        configurable: true
    });
    return AbortController;
}());
exports.AbortController = AbortController;
