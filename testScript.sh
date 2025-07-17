#!/bin/bash

BASE_URL="http://localhost:5189/api"

echo "=== Testing Workflow Engine on Port 5189 ==="
echo

# Create workflow definition
echo "1. Creating workflow definition..."
DEFINITION_RESPONSE=$(curl -s -X POST "$BASE_URL/workflow-definitions" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Order Processing Workflow",
    "description": "Basic order processing from draft to completion",
    "states": [
      {"id": "draft", "name": "Draft", "isInitial": true, "isFinal": false, "enabled": true},
      {"id": "submitted", "name": "Submitted", "isInitial": false, "isFinal": false, "enabled": true},
      {"id": "processing", "name": "Processing", "isInitial": false, "isFinal": false, "enabled": true},
      {"id": "completed", "name": "Completed", "isInitial": false, "isFinal": true, "enabled": true},
      {"id": "cancelled", "name": "Cancelled", "isInitial": false, "isFinal": true, "enabled": true}
    ],
    "actions": [
      {"id": "submit", "name": "Submit Order", "enabled": true, "fromStates": ["draft"], "toState": "submitted"},
      {"id": "process", "name": "Process Order", "enabled": true, "fromStates": ["submitted"], "toState": "processing"},
      {"id": "complete", "name": "Complete Order", "enabled": true, "fromStates": ["processing"], "toState": "completed"},
      {"id": "cancel", "name": "Cancel Order", "enabled": true, "fromStates": ["draft", "submitted"], "toState": "cancelled"}
    ]
  }')

DEFINITION_ID=$(echo $DEFINITION_RESPONSE | jq -r '.id')
echo "✓ Definition ID: $DEFINITION_ID"
echo

# Start workflow instance
echo "2. Starting workflow instance..."
INSTANCE_RESPONSE=$(curl -s -X POST "$BASE_URL/workflow-instances" \
  -H "Content-Type: application/json" \
  -d "{\"workflowDefinitionId\":\"$DEFINITION_ID\"}")

INSTANCE_ID=$(echo $INSTANCE_RESPONSE | jq -r '.id')
echo "✓ Instance ID: $INSTANCE_ID"
echo

# Test endpoints
echo "3. Testing endpoints..."
echo "   Definition endpoint: $BASE_URL/workflow-definitions/$DEFINITION_ID"
echo "   Instance endpoint: $BASE_URL/workflow-instances/$INSTANCE_ID"
echo "   Execute endpoint: $BASE_URL/workflow-instances/$INSTANCE_ID/execute"
echo "   Available actions: $BASE_URL/workflow-instances/$INSTANCE_ID/available-actions"
echo "   Current state: $BASE_URL/workflow-instances/$INSTANCE_ID/current-state"
echo

echo "=== IDs saved for manual testing ==="
echo "Definition ID: $DEFINITION_ID"
echo "Instance ID: $INSTANCE_ID"
