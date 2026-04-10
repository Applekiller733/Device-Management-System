import { DeviceType, DeviceOperatingSystem } from './enums';

export interface CreateDeviceRequest {
  name: string;
  manufacturer: string;
  type: DeviceType;
  os: DeviceOperatingSystem;
  osVersion: string;
  processor: string;
  ramAmount: string;
  description?: string;
}

export interface UpdateDeviceRequest extends CreateDeviceRequest {
  id: string;
}