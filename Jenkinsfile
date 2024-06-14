pipeline {
    agent any

    environment {
        REPO_URL = 'https://github.com/therdean/aspnet-mock.git'
        VERSION_FILE = './VERSION'
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: env.REPO_URL
            }
        }

        stage('Determine version') {
            steps {
                script {
                    def currentVersion = readFile(env.VERSION_FILE).trim()
                    def versionParts = currentVersion.split('\\.')
                    versionParts[-1] = (versionParts[-1] as int) + 1
                    def newVersion = versionParts.join('.')
                    env.VERSION_TAG = "v${newVersion}"
                    writeFile file: env.VERSION_FILE, text: newVersion
                }
            }
        }

        stage('Commit Version') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'github-pat', usernameVariable: 'USERNAME', passwordVariable: 'TOKEN')]) {
                    script {
                        sh """
                        git config --global user.name "therdean"
                        git config --global user.email "dejanristevski96@gmail.com"
                        git add .
                        git commit -m "Update version to ${env.VERSION_TAG}"
                        git push "https://\${GITHUB_USERNAME}:\${GITHUB_TOKEN}@github.com/therdean/aspnet-mock.git" main
                        """
                    }
                }
            }
        }

        stage('Tag Version') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'github-pat', usernameVariable: 'USERNAME', passwordVariable: 'TOKEN')]) {
                    script {
                        def version = env.VERSION_TAG
                        bat "git tag ${version}"
                        bat "git push https://${env.USERNAME}:${env.TOKEN}@github.com/therdean/aspnet-mock.git ${version}"
                    }
                }
            }
        }
        stage('Build') {
            steps {
                bat 'dotnet build --configuration Release'
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish --configuration Release --output C:\\Users\\Dejan.Ristevski\\Desktop\\aspnet_app\\publish'
            }
        }

        stage('Backup') {
            steps {
                script {
                    def version = env.VERSION_TAG
                    def backupDir = "C:\\Users\\Dejan.Ristevski\\Desktop\\aspnet_app\\backups\\backup_v_${version}"
                    bat "mkdir ${backupDir}"
                    bat "xcopy C:\\Users\\Dejan.Ristevski\\Desktop\\aspnet_app\\publish ${backupDir} /e /i /s"
                }
            }
        }
    }
}

github - ssh - key
