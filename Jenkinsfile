pipeline {
    agent any
    
    environment {
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 'true'
        DOTNET_CLI_HOME = "${WORKSPACE}"
    }
    
    stages {
        stage('Build') {
            agent {
                docker {
                    // Verwende das SDK-Image, da du bauen möchtest
                    image 'mcr.microsoft.com/dotnet/sdk:8.0'
                    reuseNode true
                }
            }
            steps {
                sh '''
                    ls -la
                    dotnet --version
                    dotnet restore
                    dotnet build
                '''
            }
        }
    }
}
