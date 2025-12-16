import React, { useState, useEffect } from 'react';
import './App.css';
import { Input } from '@progress/kendo-react-inputs';
import { DropDownList } from '@progress/kendo-react-dropdowns';
import { Button } from '@progress/kendo-react-buttons';
import '@progress/kendo-theme-default/dist/all.css';

interface Activity {
  [key: string]: {
    description: string;
    schedule: string;
    maxParticipants: number;
    participants: string[];
  };
}

function App() {
  const [activities, setActivities] = useState<Activity>({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [email, setEmail] = useState('');
  const [selectedActivity, setSelectedActivity] = useState('');
  const [message, setMessage] = useState<{ text: string; type: 'success' | 'error' } | null>(null);

  useEffect(() => {
    const fetchActivities = async () => {
      try {
        const response = await fetch('/api/activities');
        if (!response.ok) {
          throw new Error('Failed to fetch activities');
        }
        const data = await response.json();
        setActivities(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'An error occurred');
      } finally {
        setLoading(false);
      }
    };

    fetchActivities();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setMessage(null);

    try {
      const response = await fetch(`/api/activities/${encodeURIComponent(selectedActivity)}/signup`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email }),
      });

      if (!response.ok) {
        throw new Error('Failed to sign up');
      }

      const result = await response.json();
      setMessage({ text: result.message || 'Successfully signed up!', type: 'success' });
      setEmail('');
      setSelectedActivity('');
      
      // Refresh activities to show updated participant count
      const activitiesResponse = await fetch('/api/activities');
      const updatedActivities = await activitiesResponse.json();
      setActivities(updatedActivities);
    } catch (err) {
      setMessage({ 
        text: err instanceof Error ? err.message : 'Failed to sign up', 
        type: 'error' 
      });
    }
  };

  if (loading) return <div>Loading activities...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div className="App">
      <div className="header">
        <h1>Mergington High School</h1>
        <h2>Extracurricular Activities</h2>
      </div>
      <div className="main-content">
        <div className="activities-section">
          <h3>Available Activities</h3>
          <div className="activities-list">
            {Object.entries(activities).map(([name, activity]) => (
              <div key={name} className="activity-card">
                <h4>{name}</h4>
                <p><strong>Description:</strong> {activity.description}</p>
                <p><strong>Schedule:</strong> {activity.schedule}</p>
                <p><strong>Participants:</strong> {activity.participants.length}/{activity.maxParticipants}</p>
              </div>
            ))}
          </div>
        </div>

        <div className="signup-section">
          <h3>Sign Up for an Activity</h3>
          <form className="signup-form" onSubmit={handleSubmit}>
            <div className="form-group">
              <label htmlFor="email">Student Email:</label>
              <Input
                type="email"
                id="email"
                value={email}
                onChange={(e) => setEmail(String(e.target.value || ''))}
                required
                placeholder="your-email@mergington.edu"
                style={{ width: '100%' }}
              />
            </div>
            <div className="form-group">
              <label htmlFor="activity">Select Activity:</label>
              <DropDownList
                data={Object.keys(activities)}
                value={selectedActivity}
                onChange={(e) => setSelectedActivity(e.value)}
                defaultItem="-- Select an activity --"
                style={{ width: '100%' }}
              />
            </div>
            <Button 
              themeColor="primary" 
              type="submit"
              disabled={!email || !selectedActivity}
              style={{ width: '100%', marginTop: '1rem' }}
            >
              Sign Up
            </Button>
          </form>
          {message && (
            <div className={`message ${message.type}`}>
              {message.text}
            </div>
          )}
        </div>
      </div>
      <div className="footer">
        <p>&copy; 2025 Mergington High School</p>
      </div>
    </div>
  );
}

export default App;
