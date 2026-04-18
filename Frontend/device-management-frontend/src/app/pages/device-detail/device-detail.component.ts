import { Component, OnInit, inject } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { finalize } from 'rxjs';

import { DeviceService } from '../../services/device/device.service';
import { UserService } from '../../services/user/user.service';
import { Device } from '../../models/device';
import { User } from '../../models/user';

import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-device-detail',
  standalone: true,
  imports: [CommonModule, RouterModule,
    MatCardModule,
    MatDividerModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './device-detail.component.html',
  styleUrls: ['./device-detail.component.scss']
})
export class DeviceDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private location = inject(Location); // Used to safely go back in browser history
  private deviceService = inject(DeviceService);
  private userService = inject(UserService);

  device: Device | null = null;
  assignedUser: User | null = null;
  
  isLoading: boolean = true;
  errorMessage: string = '';

  ngOnInit(): void {
    // Grab the ID from the URL (e.g., /device-detail/1234-5678)
    const id = this.route.snapshot.paramMap.get('id');
    
    if (id) {
      this.loadDevice(id);
    } else {
      this.errorMessage = 'Invalid Device ID.';
      this.isLoading = false;
    }
  }

  loadDevice(id: string): void {
    this.deviceService.getDeviceById(id).subscribe({
      next: (deviceData) => {
        this.device = deviceData;
        
        // If a user is assigned, fetch their details before stopping the loader
        if (deviceData.assignedUserId) {
          this.loadUser(deviceData.assignedUserId);
        } else {
          this.isLoading = false; 
        }
      },
      error: (err) => {
        console.error('Error fetching device', err);
        this.errorMessage = 'Could not find this device. It may have been deleted.';
        this.isLoading = false;
      }
    });
  }

  loadUser(userId: string): void {
    this.userService.getUserById(userId)
      .pipe(finalize(() => this.isLoading = false)) // Always stop loading, even on error
      .subscribe({
        next: (userData) => this.assignedUser = userData,
        error: (err) => console.error('Failed to load assigned user', err) // Fail silently for user
      });
  }

  goBack(): void {
    this.location.back();
  }

  editDevice(): void {
    if (this.device) {
      this.router.navigate(['/device-form', this.device.id]);
    }
  }
}