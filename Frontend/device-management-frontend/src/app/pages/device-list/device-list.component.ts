import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';

import { AuthService } from '../../services/auth/auth.service';
import { DeviceService } from '../../services/device/device.service';
import { UserService } from '../../services/user/user.service';
import { Device } from '../../models/device';

import { MatTableModule } from '@angular/material/table'; 
import { MatFormFieldModule } from '@angular/material/form-field'; 
import { MatInputModule } from '@angular/material/input'; 
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule,
     MatTableModule,        
    MatFormFieldModule, 
    MatInputModule, 
    MatButtonModule], 
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit {
  private deviceService = inject(DeviceService);
  private userService = inject(UserService);
  private authService = inject(AuthService);
  private router = inject(Router);

  devices: Device[] = [];
  usersMap: Map<string, string> = new Map(); // maps user's id -> username for quick lookups
  
  isLoading: boolean = true;
  errorMessage: string = '';

  currentUserId: string | null = null;

  searchQuery: string = '';

  ngOnInit(): void {
    this.currentUserId = this.authService.currentUser()?.id || null;
    this.loadData();
  }

  loadData(): void {
    this.isLoading = true;

    // forkjoin runs both API calls in parallel and waits for both to finish
    forkJoin({
      devices: this.searchQuery ? this.deviceService.searchDevices(this.searchQuery) 
      : this.deviceService.getAllDevices(),
      users: this.userService.getAllUsers()
    }).subscribe({
      next: (result) => {
        this.devices = result.devices;
        
        // build a dictionary for O(1) lookups
        result.users.forEach(user => {
          this.usersMap.set(user.id, user.name);
        });

        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading data', err);
        this.errorMessage = 'Failed to load devices. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  // helper method to get user name from userId
  getUserName(userId: string | null): string {
    if (!userId) return 'Unassigned';
    return this.usersMap.get(userId) || 'Unknown User';
  }

  //navi
  viewDetails(id: string): void {
    this.router.navigate(['/device-detail', id]);
  }

  createNewDevice(): void {
    this.router.navigate(['/device-form']);
  }

  //delete
  deleteDevice(id: string): void {
    
    if (confirm('Are you sure you want to delete this device?')) {
      this.deviceService.deleteDevice(id).subscribe({
        next: () => {
          // optimistic ui update
          this.devices = this.devices.filter(d => d.id !== id);
        },
        error: (err) => {
          console.error('Failed to delete', err);
          alert('Error deleting device.');
        }
      });
    }
  }

  assignToMe(deviceId: string): void {
    this.deviceService.assignDevice(deviceId).subscribe({
      next: () => {
        alert('Device assigned to you!');
        this.loadData(); 
      },
      error: (err) => alert('Assignment failed: ' + err.error)
    });
  }

  unassign(deviceId: string): void {
    if (confirm('Are you sure you want to unassign this device?')) {
      this.deviceService.unassignDevice(deviceId).subscribe({
        next: () => {
          alert('Device unassigned.');
          this.loadData();
        },
        error: (err) => alert('Unassignment failed: ' + err.error)
      });
    }
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  onSearch(): void {
    this.loadData(); 
  }
}