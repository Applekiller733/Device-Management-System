import { Device } from './device';

export interface User {
    id: string;
    name: string;
    role: string;
    location: string;
    devices?: Device[];
}