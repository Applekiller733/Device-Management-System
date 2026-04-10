export interface CreateUserRequest {
    name: string;
    role: string;
    location: string;
  }
  
  export interface UpdateUserRequest extends CreateUserRequest {
    id: string;
  }