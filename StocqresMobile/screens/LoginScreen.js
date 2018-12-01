import React, {Component} from 'react';
import { Container, Header, Content, Form, Item, Input, Label, Button, Text } from 'native-base';
import {connect} from 'react-redux';

class LoginScreen extends Component{
    constructor(props){
        super(props);
        this.state = {
            login:'',
            password:''
        }
    }
    render(){
        return(
            <Container>
                <Header/>
                <Content>
                    <Form>
                        <Item fixedLabel>
                            <Label>Username</Label>
                            <Input/>
                        </Item>
                        <Item fixedLabel>
                            <Label>Password</Label>
                            <Input/>
                        </Item>
                        <Button primary>
                            <Text>SignIn</Text>
                        </Button>
                    </Form>
                   
                </Content>
            </Container>
        )
    }
}

function mapStateToProps(state){
    return{
        username: state.username,
        password: state.password
    }
}

export default connect(mapStateToProps)(LoginScreen);