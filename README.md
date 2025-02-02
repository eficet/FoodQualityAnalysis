# Food Quality Analysis

## Description

This System is built using .NET 8 with SQL Server, RabbitMQ for messaging between services, and is hosted in Docker containers. It is a **Food Quality Analysis System**.

## **Environment & System**
1. **Technologies**:
   - **Backend**: C# .NET 8 for services.
     1. QualityManager
     2. AnalysisEngine
   - **Database**: SQL Server.
     1. QualityManagerDb
     2. AnalysisEngineDb
     3. FoodQualityAnalysisTestDb - for integration tests
   - **Message Broker**: RabbitMQ.
2. **Containers**:
   - **Docker**, with two `docker-compose` configurations:
     1. `compose.yaml` - to build & host the Food Quality Analysis System.
     2. `compose.override.yaml` - for testing purposes.
3. **Tests**:
   - `FoodQualityAnalysis.Integration.Tests`
   - `FoodQualityAnalysis.Tests` - unit tests.

## **Setup & Running the Application**

### **Clone the Repository**
```bash
git clone https://github.com/eficet/FoodQualityAnalysis.git
```

### **Build the Docker Images**
```bash
docker-compose -f compose.yaml build 
```

### **Start the System**
```bash
docker-compose -f compose.yaml up -d
```

---

## **API Documentation**

### **FoodBatch Request**
| Field        | Type     |
| ------------ | -------- |
| foodName     | String   |
| serialNumber | Unique   |
| analysisType | String   |

### **Routes**

#### **Quality Manager Routes**
1. **Submit Food Batch for Analysis**
   - **POST** `http://localhost:5002/api/food`
   - Example request:
   ```json
   {
       "foodName": "Cheese",
       "serialNumber": "ABC123",
       "analysisType": "GMO"
   }
   ```

2. **Check Analysis Status**
   - **GET** `http://localhost:5002/api/food/{serialNumber}`
   - Retrieves processing status and results.

---

## **Running Tests**

### **Integration Tests**
Integration tests check:
- **End-to-end flow** of the system.
- **Service communication** via RabbitMQ.
- **Database interactions**.

#### **Run Integration Tests**
1. Ensure **SQL Server & RabbitMQ** are running:
   ```bash
   docker-compose -f compose.yaml up -d
   ```

2. **Build the test services**:
   ```bash
   docker-compose -f compose.override.yaml build
   ```

3. **Start the test services**:
   ```bash
   docker-compose -f compose.override.yaml up -d
   ```

4. **Run the integration tests**:
   ```bash
   dotnet test FoodQualityAnalysis.Integration.Tests/FoodQualityAnalysis.Integration.Tests.csproj
   ```

5. **Stop the test services after running tests**:
   ```bash
   docker-compose -f compose.override.yaml down
   ```

**📝 Note**: The **test database (`FoodQualityAnalysisTestDb`) is automatically cleaned** after each test run.

---

### **Unit Tests**
To run **unit tests**, execute:
```bash
dotnet test FoodQualityAnalysis.Tests/FoodQualityAnalysis.Tests.csproj
```

---

## **Possible Improvements**
- Automate the **integration test process** using CI/CD pipelines.

---

## **Sincerely**
**Fayiz Hamad**
