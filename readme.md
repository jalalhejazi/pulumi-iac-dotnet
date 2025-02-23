## Deploying IaC as static Azure Web App with Pulumi



### Pulumi up | Big bang deployment & Rolling update

```powershell
## This script is used to deploy the infrastructure to azure using Pulumi
.\deploy-continuous.ps1
```

![](./images/pulumi-up.png)


### Verify Cloud desired state
![](./images/deploy.png)


### Cleanup to free up cost and capacity 
![](./images/pulumi-down.png)



--- 

### [Pulumi IaC trade-off analysis](./tradeoff.md)