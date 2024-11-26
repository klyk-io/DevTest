"use strict";
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.getGlobalThis = exports.getErrorString = exports.constructUserAgent = exports.getUserAgentHeader = exports.ConsoleLogger = exports.SubjectSubscription = exports.createLogger = exports.sendMessage = exports.isArrayBuffer = exports.formatArrayBuffer = exports.getDataDetail = exports.Platform = exports.Arg = exports.VERSION = void 0;
var ILogger_1 = require("./ILogger");
var Loggers_1 = require("./Loggers");
// Version token that will be replaced by the prepack command
/** The version of the SignalR client. */
exports.VERSION = "0.0.0-DEV_BUILD";
/** @private */
var Arg = /** @class */ (function () {
    function Arg() {
    }
    Arg.isRequired = function (val, name) {
        if (val === null || val === undefined) {
            throw new Error("The '" + name + "' argument is required.");
        }
    };
    Arg.isNotEmpty = function (val, name) {
        if (!val || val.match(/^\s*$/)) {
            throw new Error("The '" + name + "' argument should not be empty.");
        }
    };
    Arg.isIn = function (val, values, name) {
        // TypeScript enums have keys for **both** the name and the value of each enum member on the type itself.
        if (!(val in values)) {
            throw new Error("Unknown " + name + " value: " + val + ".");
        }
    };
    return Arg;
}());
exports.Arg = Arg;
/** @private */
var Platform = /** @class */ (function () {
    function Platform() {
    }
    Object.defineProperty(Platform, "isBrowser", {
        // react-native has a window but no document so we should check both
        get: function () {
            return !Platform.isNode && typeof window === "object" && typeof window.document === "object";
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Platform, "isWebWorker", {
        // WebWorkers don't have a window object so the isBrowser check would fail
        get: function () {
            return !Platform.isNode && typeof self === "object" && "importScripts" in self;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Platform, "isReactNative", {
        // react-native has a window but no document
        get: function () {
            return !Platform.isNode && typeof window === "object" && typeof window.document === "undefined";
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(Platform, "isNode", {
        // Node apps shouldn't have a window object, but WebWorkers don't either
        // so we need to check for both WebWorker and window
        get: function () {
            return typeof process !== "undefined" && process.release && process.release.name === "node";
        },
        enumerable: false,
        configurable: true
    });
    return Platform;
}());
exports.Platform = Platform;
/** @private */
function getDataDetail(data, includeContent) {
    var detail = "";
    if (isArrayBuffer(data)) {
        detail = "Binary data of length " + data.byteLength;
        if (includeContent) {
            detail += ". Content: '" + formatArrayBuffer(data) + "'";
        }
    }
    else if (typeof data === "string") {
        detail = "String data of length " + data.length;
        if (includeContent) {
            detail += ". Content: '" + data + "'";
        }
    }
    return detail;
}
exports.getDataDetail = getDataDetail;
/** @private */
function formatArrayBuffer(data) {
    var view = new Uint8Array(data);
    // Uint8Array.map only supports returning another Uint8Array?
    var str = "";
    view.forEach(function (num) {
        var pad = num < 16 ? "0" : "";
        str += "0x" + pad + num.toString(16) + " ";
    });
    // Trim of trailing space.
    return str.substr(0, str.length - 1);
}
exports.formatArrayBuffer = formatArrayBuffer;
// Also in signalr-protocol-msgpack/Utils.ts
/** @private */
function isArrayBuffer(val) {
    return val && typeof ArrayBuffer !== "undefined" &&
        (val instanceof ArrayBuffer ||
            // Sometimes we get an ArrayBuffer that doesn't satisfy instanceof
            (val.constructor && val.constructor.name === "ArrayBuffer"));
}
exports.isArrayBuffer = isArrayBuffer;
/** @private */
function sendMessage(logger, transportName, httpClient, url, content, options) {
    return __awaiter(this, void 0, void 0, function () {
        var headers, _a, name, value, responseType, response;
        return __generator(this, function (_b) {
            switch (_b.label) {
                case 0:
                    headers = {};
                    _a = getUserAgentHeader(), name = _a[0], value = _a[1];
                    headers[name] = value;
                    logger.log(ILogger_1.LogLevel.Trace, "(" + transportName + " transport) sending data. " + getDataDetail(content, options.logMessageContent) + ".");
                    responseType = isArrayBuffer(content) ? "arraybuffer" : "text";
                    return [4 /*yield*/, httpClient.post(url, {
                            content: content,
                            headers: __assign(__assign({}, headers), options.headers),
                            responseType: responseType,
                            timeout: options.timeout,
                            withCredentials: options.withCredentials,
                        })];
                case 1:
                    response = _b.sent();
                    logger.log(ILogger_1.LogLevel.Trace, "(" + transportName + " transport) request complete. Response status: " + response.statusCode + ".");
                    return [2 /*return*/];
            }
        });
    });
}
exports.sendMessage = sendMessage;
/** @private */
function createLogger(logger) {
    if (logger === undefined) {
        return new ConsoleLogger(ILogger_1.LogLevel.Information);
    }
    if (logger === null) {
        return Loggers_1.NullLogger.instance;
    }
    if (logger.log !== undefined) {
        return logger;
    }
    return new ConsoleLogger(logger);
}
exports.createLogger = createLogger;
/** @private */
var SubjectSubscription = /** @class */ (function () {
    function SubjectSubscription(subject, observer) {
        this._subject = subject;
        this._observer = observer;
    }
    SubjectSubscription.prototype.dispose = function () {
        var index = this._subject.observers.indexOf(this._observer);
        if (index > -1) {
            this._subject.observers.splice(index, 1);
        }
        if (this._subject.observers.length === 0 && this._subject.cancelCallback) {
            this._subject.cancelCallback().catch(function (_) { });
        }
    };
    return SubjectSubscription;
}());
exports.SubjectSubscription = SubjectSubscription;
/** @private */
var ConsoleLogger = /** @class */ (function () {
    function ConsoleLogger(minimumLogLevel) {
        this._minLevel = minimumLogLevel;
        this.out = console;
    }
    ConsoleLogger.prototype.log = function (logLevel, message) {
        if (logLevel >= this._minLevel) {
            var msg = "[" + new Date().toISOString() + "] " + ILogger_1.LogLevel[logLevel] + ": " + message;
            switch (logLevel) {
                case ILogger_1.LogLevel.Critical:
                case ILogger_1.LogLevel.Error:
                    this.out.error(msg);
                    break;
                case ILogger_1.LogLevel.Warning:
                    this.out.warn(msg);
                    break;
                case ILogger_1.LogLevel.Information:
                    this.out.info(msg);
                    break;
                default:
                    // console.debug only goes to attached debuggers in Node, so we use console.log for Trace and Debug
                    this.out.log(msg);
                    break;
            }
        }
    };
    return ConsoleLogger;
}());
exports.ConsoleLogger = ConsoleLogger;
/** @private */
function getUserAgentHeader() {
    var userAgentHeaderName = "X-SignalR-User-Agent";
    if (Platform.isNode) {
        userAgentHeaderName = "User-Agent";
    }
    return [userAgentHeaderName, constructUserAgent(exports.VERSION, getOsName(), getRuntime(), getRuntimeVersion())];
}
exports.getUserAgentHeader = getUserAgentHeader;
/** @private */
function constructUserAgent(version, os, runtime, runtimeVersion) {
    // Microsoft SignalR/[Version] ([Detailed Version]; [Operating System]; [Runtime]; [Runtime Version])
    var userAgent = "Microsoft SignalR/";
    var majorAndMinor = version.split(".");
    userAgent += majorAndMinor[0] + "." + majorAndMinor[1];
    userAgent += " (" + version + "; ";
    if (os && os !== "") {
        userAgent += os + "; ";
    }
    else {
        userAgent += "Unknown OS; ";
    }
    userAgent += "" + runtime;
    if (runtimeVersion) {
        userAgent += "; " + runtimeVersion;
    }
    else {
        userAgent += "; Unknown Runtime Version";
    }
    userAgent += ")";
    return userAgent;
}
exports.constructUserAgent = constructUserAgent;
// eslint-disable-next-line spaced-comment
/*#__PURE__*/ function getOsName() {
    if (Platform.isNode) {
        switch (process.platform) {
            case "win32":
                return "Windows NT";
            case "darwin":
                return "macOS";
            case "linux":
                return "Linux";
            default:
                return process.platform;
        }
    }
    else {
        return "";
    }
}
// eslint-disable-next-line spaced-comment
/*#__PURE__*/ function getRuntimeVersion() {
    if (Platform.isNode) {
        return process.versions.node;
    }
    return undefined;
}
function getRuntime() {
    if (Platform.isNode) {
        return "NodeJS";
    }
    else {
        return "Browser";
    }
}
/** @private */
function getErrorString(e) {
    if (e.stack) {
        return e.stack;
    }
    else if (e.message) {
        return e.message;
    }
    return "" + e;
}
exports.getErrorString = getErrorString;
/** @private */
function getGlobalThis() {
    // globalThis is semi-new and not available in Node until v12
    if (typeof globalThis !== "undefined") {
        return globalThis;
    }
    if (typeof self !== "undefined") {
        return self;
    }
    if (typeof window !== "undefined") {
        return window;
    }
    if (typeof global !== "undefined") {
        return global;
    }
    throw new Error("could not find global");
}
exports.getGlobalThis = getGlobalThis;
