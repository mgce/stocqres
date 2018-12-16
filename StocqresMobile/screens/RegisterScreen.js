import React, {Component} from 'react';
import { StyleSheet } from 'react-native';
import { Container, Header, Content, Form, Item, Input, Label, Button, Text } from 'native-base';
import {connect} from 'react-redux';
import { Navigation } from 'react-native-navigation';
import {register} from '../ducks/authentication'

export class RegisterScreen extends Component{
    constructor(props){
        super(props);
        this.state = {
            email: 'adam@danch.pl',
            username: 'Adam',
            password: 'Adamek123',
            firstName: 'Adam',
            lastName:'Danch'
        }
    }
    registerAccount = () => {
        var command = this.state;
        this.props.register(command);
    }
    render(){
        return(
            <Container>
                <Content contentContainerStyle={styles.container}>
                    <Form style={styles.form}>
                        <Item floatingLabel>
                            <Label>Email Address</Label>
                            <Input onChangeText={(email) => this.setState({email})}/>
                        </Item>
                        <Item floatingLabel>
                            <Label>Username</Label>
                            <Input onChangeText={(username) => this.setState({username})}/>
                        </Item>
                        <Item floatingLabel>
                            <Label>First Name</Label>
                            <Input onChangeText={(firstName) => this.setState({firstName})}/>
                        </Item>
                        <Item floatingLabel>
                            <Label>Last Name</Label>
                            <Input onChangeText={(lastName) => this.setState({lastName})}/>
                        </Item>
                        <Item floatingLabel>
                            <Label>Password</Label>
                            <Input 
                            secureTextEntry={true}
                            onChangeText={(password) => this.setState({password})}/>
                        </Item>
                        <Button primary style={styles.submit}>
                            <Text onPress={() => this.registerAccount()}>Register</Text>
                        </Button>
                    </Form>
                </Content>
            </Container>
        )
    }
}

const styles = StyleSheet.create({
    container:{
      flex:1,
      paddingLeft: 15,
      paddingRight: 15,
      flexDirection:'column',
      alignItems:'center',
      justifyContent:'center'
  
    },
    submit:{
      alignSelf: 'stretch',
      borderRadius: 10,
      justifyContent: 'center',
      marginTop: 40,
    },
    form: {
      alignSelf: 'stretch',
      backgroundColor: '#ffffff',
      borderColor: '#d6d7da',
      borderRadius: 10,
      borderWidth: 1,
      padding: 10,
      paddingBottom: 30,
    },
    registerText:{
        flexDirection: 'column',
        fontSize: 12,
        justifyContent: 'center',
        paddingTop: 10
        
    }
  });

const mapStateToProps = (state) => {
    return{
        username: state.authentication.initialState.username,
        password: state.authentication.initialState.password
    }
}

const mapDispatchToProps = {
    register
};

export default connect(null, mapDispatchToProps)(RegisterScreen);