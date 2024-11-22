import DeviceImageModel from "./DeviceImageModel";

export default interface CreateDeviceModel {
  name: string;
  model: string;
  description: string;
  images: Array<DeviceImageModel>;
}
