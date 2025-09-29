# TripWhiz

TripWhiz is a travel offer application that fetches accommodation and transportation data using AI and external APIs.

## 🚀 Getting Started

Before you run the application, you need to set up your API tokens.

### Publish the Batabase

Runn the update-database command to apply all the migrations.

### 🔐 Required API Keys

This application requires the following API keys:

1. **Groq API Token**  
   Used for fetching AI-generated travel data. [Groq](https://console.groq.com/keys)

2. **Pexels API Token**  
   Used for fetching royalty-free travel images. [Pexels](https://www.pexels.com/api/key/)

### 🛠 Configuration Steps

1. Open the `appsettings.json` file located in the root of the application (e.g., `TripWhiz/TripApp/appsettings.json`).
2. Add the API keys under the appropriate sections. Example:

```json
{
  "Groq": {
    "ApiKey": "YOUR_GROQ_API_KEY_HERE"
  },
  "Pexels": {
    "ApiKey": "YOUR_PEXELS_API_KEY_HERE"
  }
}
