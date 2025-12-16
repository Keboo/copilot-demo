export interface Activity {
  description: string;
  schedule: string;
  maxParticipants: number;
  participants: string[];
}

export interface SignupRequest {
  email: string;
}

export interface SignupResponse {
  success?: boolean;
  message: string;
  detail?: string;
}

export interface ActivitiesResponse {
  [key: string]: Activity;
}
