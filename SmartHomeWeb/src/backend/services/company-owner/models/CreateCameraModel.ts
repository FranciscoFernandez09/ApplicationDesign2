import DeviceImageModel from './DeviceImageModel';

export default interface CreateCameraModel {
  name: string;
  model: string;
  description: string;
  hasExternalUse: boolean;
  hasInternalUse: boolean;
  motionDetection: boolean;
  personDetection: boolean;
  images: Array<DeviceImageModel>;
}
