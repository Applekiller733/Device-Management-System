import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DeviceService } from '../../services/device/device.service';
import { UserService } from '../../services/user/user.service';
import { DeviceType, DeviceOperatingSystem } from '../../models/enums';
import { User } from '../../models/user';

@Component({
  selector: 'app-device-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './device-form.component.html',
  styleUrls: ['./device-form.component.scss']
})
export class DeviceFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private deviceService = inject(DeviceService);
  private userService = inject(UserService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  deviceForm!: FormGroup;
  isEditMode = false;
  deviceId: string | null = null;
  users: User[] = [];
  
  // Convert Enums to arrays for the dropdowns
  deviceTypes = Object.entries(DeviceType).filter(([k, v]) => typeof v === 'number');
  osTypes = Object.entries(DeviceOperatingSystem).filter(([k, v]) => typeof v === 'number');

  ngOnInit(): void {
    this.initForm();
    this.loadUsers();

    this.deviceId = this.route.snapshot.paramMap.get('id');
    if (this.deviceId) {
      this.isEditMode = true;
      this.loadDeviceData(this.deviceId);
    }
  }

  private initForm(): void {
    this.deviceForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      manufacturer: ['', Validators.required],
      type: [null, Validators.required],
      os: [null, Validators.required],
      osVersion: ['', Validators.required],
      processor: ['', Validators.required],
      ramAmount: ['', Validators.required],
      description: [''],
      assignedUserId: [null]
    });
  }

  private loadUsers(): void {
    this.userService.getAllUsers().subscribe(data => this.users = data);
  }

  private loadDeviceData(id: string): void {
    this.deviceService.getDeviceById(id).subscribe(device => {
      this.deviceForm.patchValue(device); // Automatically fills the form fields
    });
  }

  onSubmit(): void {
    if (this.deviceForm.invalid) {
      this.deviceForm.markAllAsTouched(); // Triggers validation messages
      return;
    }

    const formData = this.deviceForm.value;

    if (this.isEditMode && this.deviceId) {
      this.deviceService.updateDevice(this.deviceId, { ...formData, id: this.deviceId })
        .subscribe(() => this.router.navigate(['/']));
    } else {
      this.deviceService.createDevice(formData)
        .subscribe(() => this.router.navigate(['/']));
    }
  }
}