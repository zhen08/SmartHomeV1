namespace csharp SmartVideo.Transport

enum MEDIATYPE {
  JPG = 1,
  PNG = 2,
  MP4 = 3
}

struct MediaStruct {
  1: string deviceId,
  2: MEDIATYPE mediaType,
  3: i32 timeStamp,
  4: binary data
}


service SmartVideoLocalService {
  bool uploadMedia(1: MediaStruct media)
}
