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
var _this = this;
(function () {
    var _window = window;
    var rootCanvas = document.getElementById("root-canvas");
    var rootCtx = rootCanvas.getContext('2d');
    rootCanvas.width = 1920;
    rootCanvas.height = 1280;
    // Render targets
    // Targets should map to textures in C# / ECS, not here.
    // So no mapping is nessecary here between the two.
    var targets = new Map();
    var textures = new Map();
    _window.createTexture = function (textureId, url) { return __awaiter(_this, void 0, void 0, function () {
        var img;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    img = new Image();
                    img.id = textureId;
                    img.src = url;
                    // Wait until texture loads
                    return [4 /*yield*/, new Promise(function (resolve) {
                            img.onload = function (ev) {
                                resolve(ev);
                            };
                        })];
                case 1:
                    // Wait until texture loads
                    _a.sent();
                    textures.set(textureId, img);
                    return [2 /*return*/, textureId];
            }
        });
    }); };
    _window.deleteTexture = function (textureId) { return __awaiter(_this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            textures["delete"](textureId);
            return [2 /*return*/];
        });
    }); };
    _window.createTarget = function (targetId, width, height) { return __awaiter(_this, void 0, void 0, function () {
        var canvas, ctx;
        return __generator(this, function (_a) {
            canvas = document.createElement('canvas');
            canvas.width = 1920;
            canvas.height = 1280;
            canvas.id = targetId;
            ctx = canvas.getContext('2d');
            // schema for targets
            targets.set(targetId, {
                canvas: canvas,
                ctx: ctx
            });
            return [2 /*return*/, targetId];
        });
    }); };
    _window.deleteTarget = function (targetId) { return __awaiter(_this, void 0, void 0, function () {
        var canvas;
        return __generator(this, function (_a) {
            targets["delete"](targetId);
            canvas = document.getElementById(targetId);
            canvas.parentElement.removeChild(canvas);
            return [2 /*return*/];
        });
    }); };
    _window.drawOnTarget = function (targetId, textureId, x, y) {
        var target = targets.get(targetId);
        var texture = textures.get(textureId);
        target.ctx.drawImage(texture, x, y);
    };
    _window.drawSingleTarget = function (targetId, x, y) { return __awaiter(_this, void 0, void 0, function () {
        var target;
        return __generator(this, function (_a) {
            target = targets.get(targetId);
            console.log(target.canvas.width);
            rootCtx.drawImage(target.canvas, x, y);
            return [2 /*return*/];
        });
    }); };
    _window.clearRootCanvas = function () { return __awaiter(_this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            rootCtx.clearRect(0, 0, rootCanvas.width, rootCanvas.height);
            return [2 /*return*/];
        });
    }); };
})();
