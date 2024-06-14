pipeline {
    agent any

    environment {
        REPO_URL = 'https://github.com/therdean/aspnet-mock.git'
        VERSION_FILE = 'VERSION'
    }

    stages {
        stage('Checkout') {
            steps {
                sshagent(credentials: ['github-ssh-key']) {
                    git branch: 'main', url: env.REPO_URL
                }
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
                sshagent(credentials: ['github-ssh-key']) {
                    script {
                        bat "git config user.name 'therdean'"
                        bat "git config user.email 'dejanristevski96@gmail.com'"
                        bat "git add ${env.VERSION_FILE}"
                        bat "git commit -m 'Update version to ${env.VERSION_TAG}'"
                        bit 'git push origin main'
                    }
                }
            }
        }

        stage('Tag Version') {
            steps {
                sshagent(credentials: ['github-ssh-key']) {
                    script {
                        def version = env.VERSION_TAG
                        bat "git tag ${version}"
                        bat "git push origin ${version}"
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
