/*
 * Copyright 2014, Gregg Tavares.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:
 *
 *     * Redistributions of source code must retain the above copyright
 * notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above
 * copyright notice, this list of conditions and the following disclaimer
 * in the documentation and/or other materials provided with the
 * distribution.
 *     * Neither the name of Gregg Tavares. nor the names of his
 * contributors may be used to endorse or promote products derived from
 * this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 * OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */


using System.Threading.Tasks;
using System;

namespace M4{
    public class Computations{
        public static Task<float[]> Orthographic(float left, float right, float bottom, float top, float near, float far, float[] dst = null) {
            dst = dst ?? new float[16]; 

            dst[ 0] = 2 / (right - left);
            dst[ 1] = 0;
            dst[ 2] = 0;
            dst[ 3] = 0;
            dst[ 4] = 0;
            dst[ 5] = 2 / (top - bottom);
            dst[ 6] = 0;
            dst[ 7] = 0;
            dst[ 8] = 0;
            dst[ 9] = 0;
            dst[10] = 2 / (near - far);
            dst[11] = 0;
            dst[12] = (left + right) / (left - right);
            dst[13] = (bottom + top) / (bottom - top);
            dst[14] = (near + far) / (near - far);
            dst[15] = 1;

            return Task.FromResult(dst);
        }

        public static Task<float[]> Translate(float[] m, float tx, float ty, float tz, float[] dst = null) {
            // This is the optimized version of
            // return multiply(m, translation(tx, ty, tz), dst);
            dst = dst ?? new float[16]; 

            var m00 = m[0];
            var m01 = m[1];
            var m02 = m[2];
            var m03 = m[3];
            var m10 = m[1 * 4 + 0];
            var m11 = m[1 * 4 + 1];
            var m12 = m[1 * 4 + 2];
            var m13 = m[1 * 4 + 3];
            var m20 = m[2 * 4 + 0];
            var m21 = m[2 * 4 + 1];
            var m22 = m[2 * 4 + 2];
            var m23 = m[2 * 4 + 3];
            var m30 = m[3 * 4 + 0];
            var m31 = m[3 * 4 + 1];
            var m32 = m[3 * 4 + 2];
            var m33 = m[3 * 4 + 3];

            if (m != dst) {
            dst[ 0] = m00;
            dst[ 1] = m01;
            dst[ 2] = m02;
            dst[ 3] = m03;
            dst[ 4] = m10;
            dst[ 5] = m11;
            dst[ 6] = m12;
            dst[ 7] = m13;
            dst[ 8] = m20;
            dst[ 9] = m21;
            dst[10] = m22;
            dst[11] = m23;
            }

            dst[12] = m00 * tx + m10 * ty + m20 * tz + m30;
            dst[13] = m01 * tx + m11 * ty + m21 * tz + m31;
            dst[14] = m02 * tx + m12 * ty + m22 * tz + m32;
            dst[15] = m03 * tx + m13 * ty + m23 * tz + m33;

            return Task.FromResult(dst);
        }

        public static Task<float[]> Scale(float[] m, float sx, float sy, float sz, float[] dst = null) {
            // This is the optimized version of
            // return multiply(m, scaling(sx, sy, sz), dst);
            dst = dst ?? new float[16]; 

            dst[ 0] = sx * m[0 * 4 + 0];
            dst[ 1] = sx * m[0 * 4 + 1];
            dst[ 2] = sx * m[0 * 4 + 2];
            dst[ 3] = sx * m[0 * 4 + 3];
            dst[ 4] = sy * m[1 * 4 + 0];
            dst[ 5] = sy * m[1 * 4 + 1];
            dst[ 6] = sy * m[1 * 4 + 2];
            dst[ 7] = sy * m[1 * 4 + 3];
            dst[ 8] = sz * m[2 * 4 + 0];
            dst[ 9] = sz * m[2 * 4 + 1];
            dst[10] = sz * m[2 * 4 + 2];
            dst[11] = sz * m[2 * 4 + 3];

            if (m != dst) {
            dst[12] = m[12];
            dst[13] = m[13];
            dst[14] = m[14];
            dst[15] = m[15];
            }

            return Task.FromResult(dst);
        }

        public static Task<float[]> Translation(float tx, float ty, float tz, float[] dst = null) {
            dst = dst ?? new float[16]; 

            dst[ 0] = 1;
            dst[ 1] = 0;
            dst[ 2] = 0;
            dst[ 3] = 0;
            dst[ 4] = 0;
            dst[ 5] = 1;
            dst[ 6] = 0;
            dst[ 7] = 0;
            dst[ 8] = 0;
            dst[ 9] = 0;
            dst[10] = 1;
            dst[11] = 0;
            dst[12] = tx;
            dst[13] = ty;
            dst[14] = tz;
            dst[15] = 1;

            return Task.FromResult(dst);
        }

        public static Task<float[]> XRotate(float[] m, float angleInRadians, float[] dst = null) {
            // this is the optimized version of
            // return multiply(m, xRotation(angleInRadians), dst);
            dst = dst ?? new float[16];

            var m10 = m[4];
            var m11 = m[5];
            var m12 = m[6];
            var m13 = m[7];
            var m20 = m[8];
            var m21 = m[9];
            var m22 = m[10];
            var m23 = m[11];
            var c = (float) Math.Cos(angleInRadians);
            var s = (float) Math.Sin(angleInRadians);

            dst[4]  = c * m10 + s * m20;
            dst[5]  = c * m11 + s * m21;
            dst[6]  = c * m12 + s * m22;
            dst[7]  = c * m13 + s * m23;
            dst[8]  = c * m20 - s * m10;
            dst[9]  = c * m21 - s * m11;
            dst[10] = c * m22 - s * m12;
            dst[11] = c * m23 - s * m13;

            if (m != dst) {
                dst[ 0] = m[ 0];
                dst[ 1] = m[ 1];
                dst[ 2] = m[ 2];
                dst[ 3] = m[ 3];
                dst[12] = m[12];
                dst[13] = m[13];
                dst[14] = m[14];
                dst[15] = m[15];
            }

            return Task.FromResult(dst);
        }

        public static Task<float[]> AxisRotate(float[] m, float[] axis, float angleInRadians, float[] dst = null) {
            // This is the optimized verison of
            // return multiply(m, axisRotation(axis, angleInRadians), dst);
            dst = dst ?? new float[16];

            var x = axis[0];
            var y = axis[1];
            var z = axis[2];
            var n = (float) Math.Sqrt(x * x + y * y + z * z);
            x /= n;
            y /= n;
            z /= n;
            var xx = x * x;
            var yy = y * y;
            var zz = z * z;
            var c = (float) Math.Cos(angleInRadians);
            var s = (float) Math.Sin(angleInRadians);
            var oneMinusCosine = 1f - c;

            var r00 = xx + (1 - xx) * c;
            var r01 = x * y * oneMinusCosine + z * s;
            var r02 = x * z * oneMinusCosine - y * s;
            var r10 = x * y * oneMinusCosine - z * s;
            var r11 = yy + (1 - yy) * c;
            var r12 = y * z * oneMinusCosine + x * s;
            var r20 = x * z * oneMinusCosine + y * s;
            var r21 = y * z * oneMinusCosine - x * s;
            var r22 = zz + (1 - zz) * c;

            var m00 = m[0];
            var m01 = m[1];
            var m02 = m[2];
            var m03 = m[3];
            var m10 = m[4];
            var m11 = m[5];
            var m12 = m[6];
            var m13 = m[7];
            var m20 = m[8];
            var m21 = m[9];
            var m22 = m[10];
            var m23 = m[11];

            dst[ 0] = r00 * m00 + r01 * m10 + r02 * m20;
            dst[ 1] = r00 * m01 + r01 * m11 + r02 * m21;
            dst[ 2] = r00 * m02 + r01 * m12 + r02 * m22;
            dst[ 3] = r00 * m03 + r01 * m13 + r02 * m23;
            dst[ 4] = r10 * m00 + r11 * m10 + r12 * m20;
            dst[ 5] = r10 * m01 + r11 * m11 + r12 * m21;
            dst[ 6] = r10 * m02 + r11 * m12 + r12 * m22;
            dst[ 7] = r10 * m03 + r11 * m13 + r12 * m23;
            dst[ 8] = r20 * m00 + r21 * m10 + r22 * m20;
            dst[ 9] = r20 * m01 + r21 * m11 + r22 * m21;
            dst[10] = r20 * m02 + r21 * m12 + r22 * m22;
            dst[11] = r20 * m03 + r21 * m13 + r22 * m23;

            if (m != dst) {
            dst[12] = m[12];
            dst[13] = m[13];
            dst[14] = m[14];
            dst[15] = m[15];
            }

            return Task.FromResult(dst);
        }


    }
}