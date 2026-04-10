import { DeviceType, DeviceOperatingSystem } from './enums';

export interface Device {
    id: string; 
    name: string;
    manufacturer: string;
    type: DeviceType;
    os: DeviceOperatingSystem;
    osVersion: string;
    processor: string;
    ramAmount: string;
    description: string | null;
    assignedUserId: string | null; 
}