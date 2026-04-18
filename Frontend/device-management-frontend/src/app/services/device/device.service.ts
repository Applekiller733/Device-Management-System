import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Device } from '../../models/device';
import { CreateDeviceRequest, UpdateDeviceRequest } from '../../models/device-requests';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Devices`;

  getAllDevices(): Observable<Device[]> {
    return this.http.get<Device[]>(this.apiUrl);
  }

  getDeviceById(id: string): Observable<Device> {
    return this.http.get<Device>(`${this.apiUrl}/${id}`);
  }

  createDevice(device: CreateDeviceRequest): Observable<Device> {
    return this.http.post<Device>(this.apiUrl, device);
  }

  updateDevice(id: string, device: UpdateDeviceRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, device);
  }

  deleteDevice(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  assignDevice(id: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/assign`, {});
  }
  
  unassignDevice(id: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/unassign`, {});
  }

  searchDevices(query: string): Observable<Device[]> {
    return this.http.get<Device[]>(`${this.apiUrl}/search?q=${encodeURIComponent(query)}`);
  }
}