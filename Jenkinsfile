pipeline {
    agent any
    
    // Setze globale Umgebungsvariablen, damit .NET z.B. keine Verzeichnisse im Root erstellen muss
    environment {
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 'true'
        DOTNET_CLI_HOME = "${WORKSPACE}"
    }
    
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

        stage('Test') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:8.0'
                    reuseNode true
                }
            }
            steps {
                sh '''
                    test -f IkariDoTrainingBackend/bin/Debug/net8.0/IkariDoTrainingBackend.dll

                    dotnet test --no-build
                '''
            }
        }
    }
}
