// Copyright (c) 2009, Tom Lokovic
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//     * Redistributions of source code must retain the above copyright notice,
//       this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

using System;
namespace Midi
{
    /// <summary>
    /// Common base class for input and output devices.
    /// </summary>
    /// This base class exists mainly so that input and output devices can both go into the same
    /// kinds of MidiMessages.
    public class DeviceBase
    {
        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="manufacturerId">The manufacturer id of this device.</param>
        /// <param name="productId">The product id of this device.</param>
        /// <param name="name">The name of this device.</param>
        protected DeviceBase(UInt16 manufacturerId, UInt16 productId, string name)
        {
            this.manufacturerId = manufacturerId;
            this.productId = productId;
            this.name = name;
        }

        /// <summary>
        /// The manufacturer id of this device.
        /// </summary>
        public UInt16 ManufacturerId
        {
            get
            {
                return manufacturerId;
            }
        }

        /// <summary>
        /// The product id of this device.
        /// </summary>
        public UInt16 ProductId
        {
            get
            {
                return productId;
            }
        }

        /// <summary>
        /// The name of this device.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// A string containing the name, manufacturer id, and product id of this device.
        /// </summary>
        public string Spec
        {
            get
            {
                return String.Format("{0} (manuf: {1} prod: {2})", name, manufacturerId, productId);
            }
        }

        private UInt16 manufacturerId;
        private UInt16 productId;
        private string name;
    }

    /// <summary>
    /// Exception thrown when an operation on a MIDI device cannot be satisfied.
    /// </summary>
    public class DeviceException : System.ApplicationException
    {
        /// <summary>
        /// Constructs exception with a specific error message.
        /// </summary>
        /// <param name="message"></param>
        public DeviceException(string message) { }
    }
}
