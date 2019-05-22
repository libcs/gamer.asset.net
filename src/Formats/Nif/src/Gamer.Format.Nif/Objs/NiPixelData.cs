/* Copyright (c) 2006, NIF File Format Library and Tools
All rights reserved.  Please see niflib.h for license. */

//-----------------------------------NOTICE----------------------------------//
// Some of this file is automatically filled in by a Python script.  Only    //
// add custom code in the designated areas or it will be overwritten during  //
// the next update.                                                          //
//-----------------------------------NOTICE----------------------------------//

using System;
using System.IO;
using System.Collections.Generic;


namespace Niflib {

/*! A texture. */
public class NiPixelData : NiPixelFormat {
	//Definition of TYPE constant
	public static readonly Type_ TYPE = new Type_("NiPixelData", NiPixelFormat.TYPE);
	/*!  */
	internal NiPalette palette;
	/*!  */
	internal uint numMipmaps;
	/*!  */
	internal uint bytesPerPixel;
	/*!  */
	internal IList<MipMap> mipmaps;
	/*!  */
	internal uint numPixels;
	/*!  */
	internal uint numFaces;
	/*!  */
	internal IList<byte> pixelData;

	public NiPixelData() {
	palette = null;
	numMipmaps = (uint)0;
	bytesPerPixel = (uint)0;
	numPixels = (uint)0;
	numFaces = (uint)1;
}

/*!
 * Used to determine the type of a particular instance of this object.
 * \return The type constant for the actual type of the object.
 */
public override Type_ GetType() => TYPE;

/*!
 * A factory function used during file reading to create an instance of this type of object.
 * \return A pointer to a newly allocated instance of this type of object.
 */
public static NiObject Create() => new NiPixelData();

/*! NIFLIB_HIDDEN function.  For internal use only. */
internal override void Read(IStream s, List<uint> link_stack, NifInfo info) {

	uint block_num;
	base.Read(s, link_stack, info);
	Nif.NifStream(out block_num, s, info);
	link_stack.Add(block_num);
	Nif.NifStream(out numMipmaps, s, info);
	Nif.NifStream(out bytesPerPixel, s, info);
	mipmaps = new MipMap[numMipmaps];
	for (var i1 = 0; i1 < mipmaps.Count; i1++) {
		Nif.NifStream(out mipmaps[i1].width, s, info);
		Nif.NifStream(out mipmaps[i1].height, s, info);
		Nif.NifStream(out mipmaps[i1].offset, s, info);
	}
	Nif.NifStream(out numPixels, s, info);
	if (info.version >= 0x0A030006) {
		Nif.NifStream(out numFaces, s, info);
	}
	if (info.version <= 0x0A030005) {
		pixelData = new byte[numPixels];
		for (var i2 = 0; i2 < pixelData.Count; i2++) {
			Nif.NifStream(out pixelData[i2], s, info);
		}
	}
	if (info.version >= 0x0A030006) {
		pixelData = new byte[(numPixels * numFaces)];
		for (var i2 = 0; i2 < pixelData.Count; i2++) {
			Nif.NifStream(out (byte)pixelData[i2], s, info);
		}
	}

}

/*! NIFLIB_HIDDEN function.  For internal use only. */
internal override void Write(OStream s, Dictionary<NiObject, uint> link_map, List<NiObject> missing_link_stack, NifInfo info) {

	base.Write(s, link_map, missing_link_stack, info);
	numPixels = (uint)pixelData.Count;
	numMipmaps = (uint)mipmaps.Count;
	WriteRef((NiObject)palette, s, info, link_map, missing_link_stack);
	Nif.NifStream(numMipmaps, s, info);
	Nif.NifStream(bytesPerPixel, s, info);
	for (var i1 = 0; i1 < mipmaps.Count; i1++) {
		Nif.NifStream(mipmaps[i1].width, s, info);
		Nif.NifStream(mipmaps[i1].height, s, info);
		Nif.NifStream(mipmaps[i1].offset, s, info);
	}
	Nif.NifStream(numPixels, s, info);
	if (info.version >= 0x0A030006) {
		Nif.NifStream(numFaces, s, info);
	}
	if (info.version <= 0x0A030005) {
		for (var i2 = 0; i2 < pixelData.Count; i2++) {
			Nif.NifStream(pixelData[i2], s, info);
		}
	}
	if (info.version >= 0x0A030006) {
		for (var i2 = 0; i2 < pixelData.Count; i2++) {
			Nif.NifStream((byte)pixelData[i2], s, info);
		}
	}

}

/*!
 * Summarizes the information contained in this object in English.
 * \param[in] verbose Determines whether or not detailed information about large areas of data will be printed cs.
 * \return A string containing a summary of the information within the object in English.  This is the function that Niflyze calls to generate its analysis, so the output is the same.
 */
public override string AsString(bool verbose = false) {

	var s = new System.Text.StringBuilder();
	uint array_output_count = 0;
	s.Append(base.AsString());
	numPixels = (uint)pixelData.Count;
	numMipmaps = (uint)mipmaps.Count;
	s.AppendLine($"  Palette:  {palette}");
	s.AppendLine($"  Num Mipmaps:  {numMipmaps}");
	s.AppendLine($"  Bytes Per Pixel:  {bytesPerPixel}");
	array_output_count = 0;
	for (var i1 = 0; i1 < mipmaps.Count; i1++) {
		if (!verbose && (array_output_count > Nif.MAXARRAYDUMP)) {
			s.AppendLine("<Data Truncated. Use verbose mode to see complete listing.>");
			break;
		}
		s.AppendLine($"    Width:  {mipmaps[i1].width}");
		s.AppendLine($"    Height:  {mipmaps[i1].height}");
		s.AppendLine($"    Offset:  {mipmaps[i1].offset}");
	}
	s.AppendLine($"  Num Pixels:  {numPixels}");
	s.AppendLine($"  Num Faces:  {numFaces}");
	array_output_count = 0;
	for (var i1 = 0; i1 < pixelData.Count; i1++) {
		if (!verbose && (array_output_count > Nif.MAXARRAYDUMP)) {
			s.AppendLine("<Data Truncated. Use verbose mode to see complete listing.>");
			break;
		}
		if (!verbose && (array_output_count > Nif.MAXARRAYDUMP)) {
			break;
		}
		s.AppendLine($"    Pixel Data[{i1}]:  {pixelData[i1]}");
		array_output_count++;
	}
	return s.ToString();

}

/*! NIFLIB_HIDDEN function.  For internal use only. */
internal override void FixLinks(Dictionary<uint, NiObject> objects, List<uint> link_stack, List<NiObject> missing_link_stack, NifInfo info) {

	base.FixLinks(objects, link_stack, missing_link_stack, info);
	palette = FixLink<NiPalette>(objects, link_stack, missing_link_stack, info);

}

/*! NIFLIB_HIDDEN function.  For internal use only. */
internal override List<NiObject> GetRefs() {
	var refs = base.GetRefs();
	if (palette != null)
		refs.Add((NiObject)palette);
	return refs;
}

/*! NIFLIB_HIDDEN function.  For internal use only. */
internal override List<NiObject> GetPtrs() {
	var ptrs = base.GetPtrs();
	return ptrs;
}


}

}