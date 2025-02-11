pipeline {
    agent any

    stages {
        stage('Build') {
            agent {
                docker {
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
