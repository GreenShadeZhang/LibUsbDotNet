﻿// Copyright © 2006-2010 Travis Robinson. All rights reserved.
// 
// website: http://sourceforge.net/projects/libusbdotnet
// e-mail:  libusbdotnet@gmail.com
// 
// This program is free software; you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the
// Free Software Foundation; either version 2 of the License, or 
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
// for more details.
// 
// You should have received a copy of the GNU General Public License along
// with this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA. or 
// visit www.gnu.org.
// 
// 
using LibUsbDotNet.Main;

namespace LibUsbDotNet.LibUsb
{
    // Implements functionality for the UsbDevice class related to endpoints.
    public partial class UsbDevice
    {
        protected readonly UsbEndpointList mActiveEndpoints = new UsbEndpointList();

        /// <inheritdoc/>
        public UsbEndpointList ActiveEndpoints => this.mActiveEndpoints;

        /// <summary>
        /// Opens a <see cref="EndpointType.Bulk"/> endpoint for reading
        /// </summary>
        /// <param name="readEndpointID">Endpoint number for read operations.</param>
        /// <returns>A <see cref="UsbEndpointReader"/> class ready for reading. If the specified endpoint is already been opened, the original <see cref="UsbEndpointReader"/> class is returned.</returns>
        public LibUsbDotNet.UsbEndpointReader OpenEndpointReader(ReadEndpointID readEndpointID)
        {
            return OpenEndpointReader(readEndpointID, UsbEndpointReader.DefReadBufferSize);
        }

        /// <summary>
        /// Opens a <see cref="EndpointType.Bulk"/> endpoint for reading
        /// </summary>
        /// <param name="readEndpointID">Endpoint number for read operations.</param>
        /// <param name="readBufferSize">Size of the read buffer allocated for the <see cref="UsbEndpointReader.DataReceived"/> event.</param>
        /// <returns>A <see cref="UsbEndpointReader"/> class ready for reading. If the specified endpoint is already been opened, the original <see cref="UsbEndpointReader"/> class is returned.</returns>
        public LibUsbDotNet.UsbEndpointReader OpenEndpointReader(ReadEndpointID readEndpointID, int readBufferSize)
        {
            return OpenEndpointReader(readEndpointID, readBufferSize, EndpointType.Bulk);
        }

        /// <summary>
        /// Opens an endpoint for reading
        /// </summary>
        /// <param name="readEndpointID">Endpoint number for read operations.</param>
        /// <param name="readBufferSize">Size of the read buffer allocated for the <see cref="UsbEndpointReader.DataReceived"/> event.</param>
        /// <param name="endpointType">The type of endpoint to open.</param>
        /// <returns>A <see cref="UsbEndpointReader"/> class ready for reading. If the specified endpoint is already been opened, the original <see cref="UsbEndpointReader"/> class is returned.</returns>
        public LibUsbDotNet.UsbEndpointReader OpenEndpointReader(ReadEndpointID readEndpointID, int readBufferSize, EndpointType endpointType)
        {
            foreach (UsbEndpointBase activeEndpoint in mActiveEndpoints)
                if (activeEndpoint.EpNum == (byte)readEndpointID)
                    return (UsbEndpointReader)activeEndpoint;

            byte altIntefaceID = mClaimedInterfaces.Count == 0 ? UsbAltInterfaceSettings[0] : UsbAltInterfaceSettings[mClaimedInterfaces[mClaimedInterfaces.Count - 1]];

            UsbEndpointReader epNew = new UsbEndpointReader(this, readBufferSize, altIntefaceID, readEndpointID, endpointType);
            return (UsbEndpointReader)ActiveEndpoints.Add(epNew);
        }

        /// <summary>
        /// Opens a <see cref="EndpointType.Bulk"/> endpoint for writing
        /// </summary>
        /// <param name="writeEndpointID">Endpoint number for read operations.</param>
        /// <returns>A <see cref="UsbEndpointWriter"/> class ready for writing. If the specified endpoint is already been opened, the original <see cref="UsbEndpointWriter"/> class is returned.</returns>
        public LibUsbDotNet.UsbEndpointWriter OpenEndpointWriter(WriteEndpointID writeEndpointID)
        {
            return OpenEndpointWriter(writeEndpointID, EndpointType.Bulk);
        }

        /// <summary>
        /// Opens an endpoint for writing
        /// </summary>
        /// <param name="writeEndpointID">Endpoint number for read operations.</param>
        /// <param name="endpointType">The type of endpoint to open.</param>
        /// <returns>A <see cref="UsbEndpointWriter"/> class ready for writing. If the specified endpoint is already been opened, the original <see cref="UsbEndpointWriter"/> class is returned.</returns>
        public LibUsbDotNet.UsbEndpointWriter OpenEndpointWriter(WriteEndpointID writeEndpointID, EndpointType endpointType)
        {
            foreach (UsbEndpointBase activeEndpoint in ActiveEndpoints)
                if (activeEndpoint.EpNum == (byte)writeEndpointID)
                    return (UsbEndpointWriter)activeEndpoint;

            byte altIntefaceID = mClaimedInterfaces.Count == 0 ? UsbAltInterfaceSettings[0] : UsbAltInterfaceSettings[mClaimedInterfaces[mClaimedInterfaces.Count - 1]];

            UsbEndpointWriter epNew = new UsbEndpointWriter(this, altIntefaceID, writeEndpointID, endpointType);
            return (UsbEndpointWriter)mActiveEndpoints.Add(epNew);
        }
    }
}